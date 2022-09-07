using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YoutubeAPI.Databse;
using YoutubeAPI.Models;
using YoutubeAPI.Models.List;
using YoutubeAPI.Queue;
using YoutubeAPI.Utilities;
using static YoutubeAPI.Utilities.MailHepler;

namespace YoutubeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YoutubeController : ControllerBase
    {
        // 30 seconds
        private readonly int TIME_OUT = 30;
        private readonly string KEY;
        private readonly string USER_AGENT;
        private readonly string MAIL_SENDER;
        private readonly string PW_MAIL_SENDER;
        private readonly string MAIL_RECEIVER;





        public static IWebHostEnvironment environment;
        private readonly MusiNowDBContext _context;
        private IBackgroundTaskQueue queue;
        private readonly IConfiguration configuration;
        public YoutubeController(IBackgroundTaskQueue queue, IConfiguration configuration, MusiNowDBContext context, IWebHostEnvironment _environment)
        {
            this.queue = queue;
            this.configuration = configuration;
            this._context = context;
            environment = _environment;


            TIME_OUT = int.Parse(this.configuration.GetConnectionString("TIME_OUT"));
            KEY = this.configuration.GetConnectionString("KEY");
            USER_AGENT = this.configuration.GetConnectionString("USER_AGENT");

            MAIL_SENDER = this.configuration.GetConnectionString("MailSender");
            PW_MAIL_SENDER = this.configuration.GetConnectionString("PwMailSender");

            MAIL_RECEIVER = this.configuration.GetConnectionString("MailReceiver");

            // link



        }

        //string checkHtml = "";

        [HttpGet]
        public async Task<string> Get([FromHeader] string key, string query, RequestType type)
        {
            List<SongModel> songs = new List<SongModel>();
            bool finish = false;

            string checkHtml = "";
            string line = "";

            Models.Basic.ItemSectionRendererContent currentItem = null;


            if (type == RequestType.SEARCH)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        if (query.IndexOf("#") == 0)
                        {
                            query = query.Replace("#", "");
                        }

                        query = query.Trim().ToLower();

                        //add search history
                        var keyWord = _context.SearchHistories.Where(x => x.KeyWord == query).FirstOrDefault();
                        if (keyWord != null)
                        {
                            var repeat = keyWord.Repeat;
                            repeat += 1;
                            keyWord.Repeat = repeat;
                        }
                        else
                        {
                            _context.SearchHistories.Add(new SearchHistory()
                            {
                                KeyWord = query,
                                Repeat = 1
                            });
                        }

                        //  save
                        _context.SaveChanges();








                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/results?search_query=" + query);
                        req.UserAgent = USER_AGENT;

                        try
                        {
                            using (var resp = req.GetResponse())
                            {
                                var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                html = parseHtml(html, query);

                                if (html != string.Empty)
                                {
                                    checkHtml = html;

                                    line = "obj";
                                    var obj = JsonConvert.DeserializeObject<Models.Basic.Temperatures>(html);

                                    line = "smailObj";
                                    var smailObj = obj.Contents.TwoColumnSearchResultsRenderer;


                                    if (smailObj != null)
                                    {
                                        line = "temp";

                                        var itemRender = smailObj.PrimaryContents.SectionListRenderer1;

                                        line = "temp1";

                                        if (itemRender == null)
                                        {
                                            line = "temp2";
                                            itemRender = smailObj.PrimaryContents.SectionListRenderer2;
                                        }

                                        var temp = itemRender.Contents;

                                        line = "list";
                                        var itemListRender = temp[0].ItemSectionRenderer1;
                                        var list = new List<Models.Basic.ItemSectionRendererContent>();


                                        if (itemListRender == null)
                                        {
                                            itemListRender = temp[0].ItemSectionRenderer2;

                                            foreach (var item in temp)
                                            {
                                                if (item.ItemSectionRenderer2 != null)
                                                {
                                                    if (item.ItemSectionRenderer2.Contents.Count() > 10)
                                                    {
                                                        list = item.ItemSectionRenderer2.Contents;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var item in temp)
                                            {
                                                if (item.ItemSectionRenderer1 != null)
                                                {
                                                    if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                                    {
                                                        list = item.ItemSectionRenderer1.Contents;
                                                        break;
                                                    }
                                                }
                                            }
                                        }







                                        foreach (var item in list)
                                        {
                                            currentItem = item;


                                            // parse to obj
                                            if (item.VideoRenderer != null)
                                            {


                                                if (item.VideoRenderer.LengthText != null)
                                                {
                                                    if (item.VideoRenderer.PublishedTimeText != null)
                                                    {
                                                        var time = item.VideoRenderer.LengthText.SimpleText;
                                                        if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            var trackId = item.VideoRenderer.VideoId;
                                                            var title = item.VideoRenderer.Title.Runs[0].Text;


                                                            if (time.IndexOf(":") < 0)
                                                            {
                                                                time = "00:00:" + time;
                                                            }
                                                            else if (time.Split(":").Length == 2)
                                                            {
                                                                time = "00:" + time;
                                                            }
                                                            var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                            var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                            var artist = item.VideoRenderer.OwnerText.Runs[0].Text;

                                                            var listImg = item.VideoRenderer.Thumbnail.Thumbnails;
                                                            var image = listImg[listImg.Count() - 1].Url;


                                                            songs.Add(new SongModel()
                                                            {
                                                                TrackId = trackId,
                                                                Duration = duration,
                                                                Title = title,
                                                                //CreateAt = creatAt,
                                                                Artist = artist,
                                                                Image = image

                                                            });
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {

                                                if (item.RichRenderer != null)
                                                {
                                                    var smallItem = item.RichRenderer.Content.VideoRenderer;



                                                    if (smallItem != null)
                                                    {


                                                        if (smallItem.LengthText != null)
                                                        {
                                                            if (smallItem.PublishedTimeText != null)
                                                            {
                                                                var time = smallItem.LengthText.SimpleText;
                                                                if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    var trackId = smallItem.VideoId;
                                                                    var title = smallItem.Title.Runs[0].Text;


                                                                    if (time.IndexOf(":") < 0)
                                                                    {
                                                                        time = "00:00:" + time;
                                                                    }
                                                                    else if (time.Split(":").Length == 2)
                                                                    {
                                                                        time = "00:" + time;
                                                                    }
                                                                    var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                                    var creatAt = smallItem.PublishedTimeText.SimpleText;
                                                                    var artist = smallItem.OwnerText.Runs[0].Text;

                                                                    var listImg = smallItem.Thumbnail.Thumbnails;
                                                                    var image = listImg[listImg.Count() - 1].Url;


                                                                    songs.Add(new SongModel()
                                                                    {
                                                                        TrackId = trackId,
                                                                        Duration = duration,
                                                                        Title = title,
                                                                        //CreateAt = creatAt,
                                                                        Artist = artist,
                                                                        Image = image

                                                                    });
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }


                                        }

                                    }
                                    else
                                    {
                                        var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[0].TabRenderer.Content.RichGridRenderer.Contents;




                                        foreach (var item in list)
                                        {
                                            //currentItem = item;
                                            // parse to obj
                                            if (item.RichItemRenderer != null)
                                            {
                                                if (item.RichItemRenderer.Content.VideoRenderer != null)
                                                {
                                                    if (item.RichItemRenderer.Content.VideoRenderer.LengthText != null)
                                                    {
                                                        if (item.RichItemRenderer.Content.VideoRenderer.PublishedTimeText != null)
                                                        {
                                                            var trackId = item.RichItemRenderer.Content.VideoRenderer.VideoId;
                                                            var title = item.RichItemRenderer.Content.VideoRenderer.Title.Runs[0].Text;

                                                            var time = item.RichItemRenderer.Content.VideoRenderer.LengthText.SimpleText;
                                                            if (time.IndexOf(":") < 0)
                                                            {
                                                                time = "00:00:" + time;
                                                            }
                                                            else if (time.Split(":").Length == 2)
                                                            {
                                                                time = "00:" + time;
                                                            }
                                                            var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                            var creatAt = item.RichItemRenderer.Content.VideoRenderer.PublishedTimeText.SimpleText;
                                                            var artist = item.RichItemRenderer.Content.VideoRenderer.OwnerText.Runs[0].Text;

                                                            var listImg = item.RichItemRenderer.Content.VideoRenderer.Thumbnail.Thumbnails;
                                                            var image = listImg[listImg.Count() - 1].Url;


                                                            songs.Add(new SongModel()
                                                            {
                                                                TrackId = trackId,
                                                                Duration = duration,
                                                                Title = title,
                                                                //CreateAt = creatAt,
                                                                Artist = artist,
                                                                Image = image

                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }







                                finish = true;
                            }
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                            //{
                            //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            //}

                            //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                            //if (!Directory.Exists(folder))
                            //{
                            //    Directory.CreateDirectory(folder);
                            //}

                            //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                            //var fileName = Path.Combine(folder, nameFile);


                            //new FileWriter().WriteData(checkHtml, fileName);

                            //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                            //var subject = "[MusiNow Report] Bug on parse HTML";


                            //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);



                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on SEARCH song API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + line + "<br/>" + JsonConvert.SerializeObject(currentItem) + "<br/>" + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            ////var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            checkHtml = "";

                        }
                    }
                    else
                    {
                        finish = true;
                    }




                });
            }
            else if (type == RequestType.PLAY_LIST)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //PlaylistVideoListRendererContent ErrorItem = new PlaylistVideoListRendererContent();
                        //ErrorItem.PlaylistVideoRenderer = new PlaylistVideoRenderer();
                        //ErrorItem.PlaylistVideoRenderer.VideoId = "TEST";


                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/playlist?list=" + query);
                        req.UserAgent = USER_AGENT;



                        try
                        {
                            using (var resp = req.GetResponse())
                            {
                                var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                //checkHtml = html;

                                html = parseHtml(html, query);

                                if (html != string.Empty)
                                {
                                    var obj = JsonConvert.DeserializeObject<Root>(html);

                                    var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[0].TabRenderer.Content.SectionListRenderer.Contents[0].ItemSectionRenderer.Contents[0].PlaylistVideoListRenderer.Contents;

                                    foreach (var item in list)
                                    {
                                        //ErrorItem = item;
                                        // parse to obj
                                        //if (item.VideoRenderer != null)
                                        //{

                                        if (item.PlaylistVideoRenderer != null)
                                        {
                                            if (item.PlaylistVideoRenderer.LengthText != null)
                                            {
                                                if (item.PlaylistVideoRenderer.ShortBylineText != null)
                                                {
                                                    if (item.PlaylistVideoRenderer.ShortBylineText.Runs != null)
                                                    {
                                                        //var time = item.PlaylistVideoRenderer.LengthText.SimpleText;
                                                        //if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                        //{

                                                        //}
                                                        //else
                                                        //{
                                                        var trackId = item.PlaylistVideoRenderer.VideoId;
                                                        var title = item.PlaylistVideoRenderer.Title.Runs[0].Text;
                                                        if (title == null)
                                                        {
                                                            title = item.PlaylistVideoRenderer.Title.Runs[0].Text;
                                                            if (title == null)
                                                            {
                                                                title = item.PlaylistVideoRenderer.Title.Accessibility.AccessibilityDataAccessibilityData.Label;
                                                            }
                                                        }


                                                        //if (time.IndexOf(":") < 0)
                                                        //{
                                                        //    time = "00:00:" + time;
                                                        //}
                                                        //else if (time.Split(":").Length == 2)
                                                        //{
                                                        //    time = "00:" + time;
                                                        //}



                                                        //var duration = (long)TimeSpan.Parse(time).TotalSeconds;

                                                        var duration = item.PlaylistVideoRenderer.LengthSeconds;

                                                        //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                        var artist = item.PlaylistVideoRenderer.ShortBylineText.Runs[0].Text;

                                                        var listImg = item.PlaylistVideoRenderer.Thumbnail.Thumbnails;
                                                        var image = listImg[listImg.Count() - 1].Url;


                                                        songs.Add(new SongModel()
                                                        {
                                                            TrackId = trackId,
                                                            Duration = duration,
                                                            Title = title,
                                                            //CreateAt = creatAt,
                                                            Artist = artist,
                                                            Image = image

                                                        });
                                                        //}

                                                    }

                                                }

                                            }
                                        }

                                        //}
                                    }
                                }






                                finish = true;
                            }
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET PLAY LIST API";

                            var msg = "";

                            msg = "Detail: " + e.Message + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }
                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.SEARCH_PLAY_LIST)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        if (query.IndexOf("#") == 0)
                        {
                            query = query.Replace("#", "");
                        }

                        query = query.Trim().ToLower();
                        // add search history
                        var keyWord = _context.SearchHistories.Where(x => x.KeyWord == query).FirstOrDefault();
                        if (keyWord != null)
                        {
                            var repeat = keyWord.Repeat;
                            repeat += 1;
                            keyWord.Repeat = repeat;
                        }
                        else
                        {
                            _context.SearchHistories.Add(new SearchHistory()
                            {
                                KeyWord = query,
                                Repeat = 1
                            });
                        }

                        // save
                        _context.SaveChanges();







                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/results?search_query=" + query + "&sp=EgIQAw%253D%253D");
                        req.UserAgent = USER_AGENT;

                        try
                        {
                            using (var resp = req.GetResponse())
                            {
                                var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                html = parseHtml(html, query);

                                if (html != string.Empty)
                                {

                                    checkHtml = html;

                                    line = "obj";

                                    var obj = JsonConvert.DeserializeObject<Models.PlayList.Temperatures>(html);

                                    var itemRender = obj.Contents.TwoColumnSearchResultsRenderer.PrimaryContents.SectionListRenderer1;

                                    if (itemRender == null)
                                    {
                                        itemRender = obj.Contents.TwoColumnSearchResultsRenderer.PrimaryContents.SectionListRenderer2;
                                    }

                                    line = "temp";

                                    var temp = itemRender.Contents;

                                    line = "list";

                                    //var list = temp[0].ItemSectionRenderer1.Contents;


                                    //foreach (var item in temp)
                                    //{
                                    //    if (item.ItemSectionRenderer1 != null)
                                    //    {
                                    //        if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                    //        {
                                    //            list = item.ItemSectionRenderer1.Contents;
                                    //            break;
                                    //        }
                                    //    }
                                    //}



                                    var itemListRender = temp[0].ItemSectionRenderer1;
                                    var list = new List<Models.PlayList.ItemSectionRendererContent>();


                                    if (itemListRender == null)
                                    {
                                        itemListRender = temp[0].ItemSectionRenderer2;

                                        foreach (var item in temp)
                                        {
                                            if (item.ItemSectionRenderer2 != null)
                                            {
                                                if (item.ItemSectionRenderer2.Contents.Count() > 10)
                                                {
                                                    list = item.ItemSectionRenderer2.Contents;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in temp)
                                        {
                                            if (item.ItemSectionRenderer1 != null)
                                            {
                                                if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                                {
                                                    list = item.ItemSectionRenderer1.Contents;
                                                    break;
                                                }
                                            }
                                        }
                                    }



                                    foreach (var item in list)
                                    {

                                        //currentItem = item;
                                        // parse to obj
                                        if (item.PlaylistRenderer != null)
                                        {
                                            if (item.PlaylistRenderer.PlaylistId != null)
                                            {
                                                if (item.PlaylistRenderer.ShortBylineText.Runs != null)
                                                {
                                                    var trackId = item.PlaylistRenderer.PlaylistId;
                                                    var title = item.PlaylistRenderer.Title.SimpleText;



                                                    //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                    var artist = item.PlaylistRenderer.ShortBylineText.Runs[0].Text;

                                                    var listImg = item.PlaylistRenderer.Thumbnails;
                                                    var image = listImg[listImg.Count() - 1].Thumbnails[0].Url;

                                                    var videoCount = item.PlaylistRenderer.VideoCount;


                                                    songs.Add(new SongModel()
                                                    {
                                                        TrackId = trackId,
                                                        //Duration = duration,
                                                        Title = title,
                                                        //CreateAt = creatAt,
                                                        Artist = artist,
                                                        Image = image,
                                                        VideoCount = videoCount

                                                    });
                                                }

                                            }

                                        }

                                        else
                                        {
                                            if (item.RichRenderer != null)
                                            {
                                                var smallItem = item.RichRenderer.Content.PlaylistRenderer;

                                                if (smallItem != null)
                                                {
                                                    if (smallItem.PlaylistId != null)
                                                    {
                                                        if (smallItem.ShortBylineText.Runs != null)
                                                        {
                                                            var trackId = smallItem.PlaylistId;
                                                            var title = smallItem.Title.SimpleText;



                                                            //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                            var artist = smallItem.ShortBylineText.Runs[0].Text;

                                                            var listImg = smallItem.Thumbnails;
                                                            var image = listImg[listImg.Count() - 1].Thumbnails[0].Url;

                                                            var videoCount = smallItem.VideoCount;


                                                            songs.Add(new SongModel()
                                                            {
                                                                TrackId = trackId,
                                                                //Duration = duration,
                                                                Title = title,
                                                                //CreateAt = creatAt,
                                                                Artist = artist,
                                                                Image = image,
                                                                VideoCount = videoCount

                                                            });
                                                        }

                                                    }

                                                }
                                            }
                                        }


                                    }
                                }






                                finish = true;
                            }
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on SEARCH PLAY LIST API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);





                            ///////////////
                            ///


                            //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                            //{
                            //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            //}

                            //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                            //if (!Directory.Exists(folder))
                            //{
                            //    Directory.CreateDirectory(folder);
                            //}

                            //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                            //var fileName = Path.Combine(folder, nameFile);


                            //new FileWriter().WriteData(checkHtml, fileName);

                            //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                            //var subject = "[MusiNow Report] Bug on parse HTML";


                            //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);



                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on Bug on SEARCH PLAY LIST API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + line + "<br/>" + JsonConvert.SerializeObject(currentItem) + "<br/>" + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            ////var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            checkHtml = "";

                        }
                    }
                    else
                    {
                        finish = true;
                    }




                });
            }
            else if (type == RequestType.TRENDING)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());


                        if (link != null && link.Length != 0)
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
                            req.UserAgent = USER_AGENT;

                            try
                            {
                                using (var resp = req.GetResponse())
                                {
                                    var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                    html = parseHtml(html, query);

                                    checkHtml = html;

                                    if (html != string.Empty)
                                    {
                                        var obj = JsonConvert.DeserializeObject<Models.Trending.TrendingModel>(html);

                                        var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[1].TabRenderer.Content.SectionListRenderer.Contents[0].ItemSectionRenderer.Contents[0].ShelfRenderer.Content.ExpandedShelfContentsRenderer.Items;

                                        foreach (var item in list)
                                        {
                                            var tmp = item;
                                            // parse to obj
                                            //if (item.VideoRenderer != null)
                                            //{
                                            if (tmp.VideoRenderer.LengthText != null)
                                            {
                                                if (tmp.VideoRenderer.ShortBylineText != null)
                                                {
                                                    if (tmp.VideoRenderer.ShortBylineText.Runs != null)
                                                    {
                                                        var time = tmp.VideoRenderer.LengthText.SimpleText;
                                                        if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            var trackId = tmp.VideoRenderer.VideoId;
                                                            var title = tmp.VideoRenderer.Title.Runs[0].Text;
                                                            if (title == null)
                                                            {
                                                                title = tmp.VideoRenderer.Title.Accessibility.AccessibilityData.Label;
                                                            }


                                                            if (time.IndexOf(":") < 0)
                                                            {
                                                                time = "00:00:" + time;
                                                            }
                                                            else if (time.Split(":").Length == 2)
                                                            {
                                                                time = "00:" + time;
                                                            }



                                                            var duration = (long)TimeSpan.Parse(time).TotalSeconds;
                                                            //var duration = item.VideoRenderer.;

                                                            //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                            var artist = tmp.VideoRenderer.ShortBylineText.Runs[0].Text;

                                                            var listImg = tmp.VideoRenderer.Thumbnail.Thumbnails;
                                                            var image = listImg[listImg.Count() - 1].Url;


                                                            songs.Add(new SongModel()
                                                            {
                                                                TrackId = trackId,
                                                                Duration = duration,
                                                                Title = title,
                                                                //CreateAt = creatAt,
                                                                Artist = artist,
                                                                Image = image

                                                            });
                                                        }

                                                    }

                                                }

                                            }

                                            //}
                                        }
                                    }







                                    finish = true;
                                }
                            }
                            catch (Exception e)
                            {
                                finish = true;

                                //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                                //{
                                //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                                //}

                                //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                                //if (!Directory.Exists(folder))
                                //{
                                //    Directory.CreateDirectory(folder);
                                //}

                                //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                                //var fileName = Path.Combine(folder, nameFile);


                                //new FileWriter().WriteData(checkHtml, fileName);

                                // send mail to Hieu
                                //var subject = "[MusiNow Report] Bug on GET TRENDING LIST API";
                                //var msg = "";
                                //msg = "Detail: " + e.Message + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                                //if (e.GetType() == typeof(WebException))
                                //{
                                //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                                //}

                                //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            }
                        }
                        else
                        {
                            finish = true;
                        }


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.RELATED)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        //link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());

                        //if (link != null && link.Length != 0)
                        //{
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=" + query);
                        req.UserAgent = USER_AGENT;

                        try
                        {
                            using (var resp = req.GetResponse())
                            {
                                var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                html = parseHtml(html, query);

                                if (html != string.Empty)
                                {
                                    var obj = JsonConvert.DeserializeObject<Models.Related.JsonModelRelated>(html);

                                    var list = obj.Contents.TwoColumnWatchNextResults.SecondaryResults.SecondaryResults.Results;

                                    foreach (var item in list)
                                    {
                                        if (item.CompactVideoRenderer != null)
                                        {
                                            if (item.CompactVideoRenderer.VideoId != null)
                                            {
                                                if (item.CompactVideoRenderer.LengthText != null)
                                                {
                                                    if (item.CompactVideoRenderer.ShortBylineText != null)
                                                    {
                                                        if (item.CompactVideoRenderer.ShortBylineText.Runs != null)
                                                        {
                                                            var time = item.CompactVideoRenderer.LengthText.SimpleText;
                                                            if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                var trackId = item.CompactVideoRenderer.VideoId;
                                                                var title = item.CompactVideoRenderer.Title.SimpleText;
                                                                if (title == null)
                                                                {
                                                                    title = item.CompactVideoRenderer.Title.Accessibility.AccessibilityData.Label;
                                                                }


                                                                if (time.IndexOf(":") < 0)
                                                                {
                                                                    time = "00:00:" + time;
                                                                }
                                                                else if (time.Split(":").Length == 2)
                                                                {
                                                                    time = "00:" + time;
                                                                }



                                                                var duration = (long)TimeSpan.Parse(time).TotalSeconds;
                                                                //var duration = item.VideoRenderer.;

                                                                //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                                var artist = item.CompactVideoRenderer.ShortBylineText.Runs[0].Text;

                                                                var listImg = item.CompactVideoRenderer.Thumbnail.Thumbnails;
                                                                var image = listImg[listImg.Count() - 1].Url;


                                                                songs.Add(new SongModel()
                                                                {
                                                                    TrackId = trackId,
                                                                    Duration = duration,
                                                                    Title = title,
                                                                    //CreateAt = creatAt,
                                                                    Artist = artist,
                                                                    Image = image

                                                                });
                                                            }

                                                        }

                                                    }

                                                }
                                            }
                                        }
                                        // parse to obj
                                        //if (item.VideoRenderer != null)
                                        //{


                                        //}
                                    }
                                }







                                finish = true;
                            }
                        }
                        catch (Exception e)
                        {
                            finish = true;



                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET Related LIST API";
                            var msg = "";
                            msg = "Detail: " + e.Message + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }
                        //}
                        //else
                        //{
                        //    finish = true;
                        //}


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.VIDEO)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        //link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());

                        //if (link != null && link.Length != 0)
                        //{



                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=" + query);
                        req.UserAgent = USER_AGENT;

                        using (var resp = req.GetResponse())
                        {
                            try
                            {
                                var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

                                html = parseHtmlVideo(html, query);

                                if (html != string.Empty)
                                {
                                    var obj = JsonConvert.DeserializeObject<Models.JsonModelTrack>(html);

                                    //var list = obj.Contents.TwoColumnWatchNextResults.SecondaryResults.SecondaryResults.Results;
                                    var item = obj.VideoDetails;

                                    var trackId = query;
                                    var title = item.Title;






                                    var duration = item.LengthSeconds;
                                    //var duration = item.VideoRenderer.;

                                    //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                    var artist = item.Author;

                                    var listImg = item.Thumbnail.Thumbnails;
                                    var image = listImg[listImg.Count() - 1].Url;


                                    songs.Add(new SongModel()
                                    {
                                        TrackId = trackId,
                                        Duration = duration,
                                        Title = title,
                                        //CreateAt = creatAt,
                                        Artist = artist,
                                        Image = image

                                    });
                                }






                                finish = true;
                            }
                            catch (Exception e)
                            {
                                finish = true;



                                // send mail to Hieu
                                var subject = "[MusiNow Report] Bug on GET VIDEO TRACK API";
                                var msg = "";
                                msg = "Detail: " + e.Message + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                                if (e.GetType() == typeof(WebException))
                                {
                                    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                    msg = "Detail: " + error + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                                }

                                MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            }
                        }

                           
                        //}
                        //else
                        //{
                        //    finish = true;
                        //}


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }



            // 
            var end = DateTime.Now.AddMinutes(TIME_OUT);


            while (true)
            {
                if (DateTime.Compare(end, DateTime.Now) <= 0)
                {
                    break;
                }

                Thread.Sleep(100);
                if (finish)
                {
                    break;
                }
            }

            //var subject = "[MusiNow Report] Bug on SEARCH PLAY LIST API";
            //var msg = "<h3>Detail:</h3> <br/> Query Search: " + query + "<br/>" + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);
            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, "anhlh@digitoy.xyz", subject, msg);

            // put processing code here
            //return checkHtml;

            return JsonConvert.SerializeObject(songs);






        }

        [HttpPost("parser")]
        public async Task<string> GetHtml([FromHeader] string key, string query, RequestType type, [FromBody] string source)
        {
            List<SongModel> songs = new List<SongModel>();
            bool finish = false;

            string checkHtml = "";
            string line = "";

            Models.Basic.ItemSectionRendererContent currentItem = null;


            if (type == RequestType.SEARCH)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        if (query.IndexOf("#") == 0)
                        {
                            query = query.Replace("#", "");
                        }

                        query = query.Trim().ToLower();

                        //add search history
                        var keyWord = _context.SearchHistories.Where(x => x.KeyWord == query).FirstOrDefault();
                        if (keyWord != null)
                        {
                            var repeat = keyWord.Repeat;
                            repeat += 1;
                            keyWord.Repeat = repeat;
                        }
                        else
                        {
                            _context.SearchHistories.Add(new SearchHistory()
                            {
                                KeyWord = query,
                                Repeat = 1
                            });
                        }

                        //  save
                        _context.SaveChanges();








                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/results?search_query=" + query);
                        //req.UserAgent = USER_AGENT;

                        try
                        {
                            var html = source;

                            html = parseHtml(html, query);

                            if (html != string.Empty)
                            {
                                checkHtml = html;

                                line = "obj";
                                var obj = JsonConvert.DeserializeObject<Models.Basic.Temperatures>(html);

                                line = "smailObj";
                                var smailObj = obj.Contents.TwoColumnSearchResultsRenderer;


                                if (smailObj != null)
                                {
                                    line = "temp";

                                    var itemRender = smailObj.PrimaryContents.SectionListRenderer1;

                                    line = "temp1";

                                    if (itemRender == null)
                                    {
                                        line = "temp2";
                                        itemRender = smailObj.PrimaryContents.SectionListRenderer2;
                                    }

                                    var temp = itemRender.Contents;

                                    line = "list";
                                    var itemListRender = temp[0].ItemSectionRenderer1;
                                    var list = new List<Models.Basic.ItemSectionRendererContent>();


                                    if (itemListRender == null)
                                    {
                                        itemListRender = temp[0].ItemSectionRenderer2;

                                        foreach (var item in temp)
                                        {
                                            if (item.ItemSectionRenderer2 != null)
                                            {
                                                if (item.ItemSectionRenderer2.Contents.Count() > 10)
                                                {
                                                    list = item.ItemSectionRenderer2.Contents;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in temp)
                                        {
                                            if (item.ItemSectionRenderer1 != null)
                                            {
                                                if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                                {
                                                    list = item.ItemSectionRenderer1.Contents;
                                                    break;
                                                }
                                            }
                                        }
                                    }







                                    foreach (var item in list)
                                    {
                                        currentItem = item;


                                        // parse to obj
                                        if (item.VideoRenderer != null)
                                        {


                                            if (item.VideoRenderer.LengthText != null)
                                            {
                                                if (item.VideoRenderer.PublishedTimeText != null)
                                                {
                                                    var time = item.VideoRenderer.LengthText.SimpleText;
                                                    time = time.Replace(".", ":");
                                                    if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        var trackId = item.VideoRenderer.VideoId;
                                                        var title = item.VideoRenderer.Title.Runs[0].Text;


                                                        if (time.IndexOf(":") < 0)
                                                        {
                                                            time = "00:00:" + time;
                                                        }
                                                        else if (time.Split(":").Length == 2)
                                                        {
                                                            time = "00:" + time;
                                                        }
                                                        var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                        var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                        var artist = item.VideoRenderer.OwnerText.Runs[0].Text;

                                                        var listImg = item.VideoRenderer.Thumbnail.Thumbnails;
                                                        var image = listImg[listImg.Count() - 1].Url;


                                                        songs.Add(new SongModel()
                                                        {
                                                            TrackId = trackId,
                                                            Duration = duration,
                                                            Title = title,
                                                            //CreateAt = creatAt,
                                                            Artist = artist,
                                                            Image = image

                                                        });
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {

                                            if (item.RichRenderer != null)
                                            {
                                                var smallItem = item.RichRenderer.Content.VideoRenderer;



                                                if (smallItem != null)
                                                {


                                                    if (smallItem.LengthText != null)
                                                    {
                                                        if (smallItem.PublishedTimeText != null)
                                                        {
                                                            var time = smallItem.LengthText.SimpleText;
                                                            time = time.Replace(".", ":");
                                                            if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                var trackId = smallItem.VideoId;
                                                                var title = smallItem.Title.Runs[0].Text;


                                                                if (time.IndexOf(":") < 0)
                                                                {
                                                                    time = "00:00:" + time;
                                                                }
                                                                else if (time.Split(":").Length == 2)
                                                                {
                                                                    time = "00:" + time;
                                                                }
                                                                var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                                var creatAt = smallItem.PublishedTimeText.SimpleText;
                                                                var artist = smallItem.OwnerText.Runs[0].Text;

                                                                var listImg = smallItem.Thumbnail.Thumbnails;
                                                                var image = listImg[listImg.Count() - 1].Url;


                                                                songs.Add(new SongModel()
                                                                {
                                                                    TrackId = trackId,
                                                                    Duration = duration,
                                                                    Title = title,
                                                                    //CreateAt = creatAt,
                                                                    Artist = artist,
                                                                    Image = image

                                                                });
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }


                                    }

                                }
                                else
                                {
                                    var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[0].TabRenderer.Content.RichGridRenderer.Contents;




                                    foreach (var item in list)
                                    {
                                        //currentItem = item;
                                        // parse to obj
                                        if (item.RichItemRenderer != null)
                                        {
                                            if (item.RichItemRenderer.Content.VideoRenderer != null)
                                            {
                                                if (item.RichItemRenderer.Content.VideoRenderer.LengthText != null)
                                                {
                                                    if (item.RichItemRenderer.Content.VideoRenderer.PublishedTimeText != null)
                                                    {
                                                        var trackId = item.RichItemRenderer.Content.VideoRenderer.VideoId;
                                                        var title = item.RichItemRenderer.Content.VideoRenderer.Title.Runs[0].Text;

                                                        var time = item.RichItemRenderer.Content.VideoRenderer.LengthText.SimpleText;
                                                        time = time.Replace(".", ":");
                                                        if (time.IndexOf(":") < 0)
                                                        {
                                                            time = "00:00:" + time;
                                                        }
                                                        else if (time.Split(":").Length == 2)
                                                        {
                                                            time = "00:" + time;
                                                        }
                                                        var duration = (long)TimeSpan.Parse(time).TotalSeconds;


                                                        var creatAt = item.RichItemRenderer.Content.VideoRenderer.PublishedTimeText.SimpleText;
                                                        var artist = item.RichItemRenderer.Content.VideoRenderer.OwnerText.Runs[0].Text;

                                                        var listImg = item.RichItemRenderer.Content.VideoRenderer.Thumbnail.Thumbnails;
                                                        var image = listImg[listImg.Count() - 1].Url;


                                                        songs.Add(new SongModel()
                                                        {
                                                            TrackId = trackId,
                                                            Duration = duration,
                                                            Title = title,
                                                            //CreateAt = creatAt,
                                                            Artist = artist,
                                                            Image = image

                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }







                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                            //{
                            //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            //}

                            //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                            //if (!Directory.Exists(folder))
                            //{
                            //    Directory.CreateDirectory(folder);
                            //}

                            //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                            //var fileName = Path.Combine(folder, nameFile);


                            //new FileWriter().WriteData(checkHtml, fileName);

                            //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                            //var subject = "[MusiNow Report] Bug on parse HTML";


                            //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);



                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on SEARCH song API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + line + "<br/>" + JsonConvert.SerializeObject(currentItem) + "<br/>" + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            ////var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            checkHtml = "";

                        }
                    }
                    else
                    {
                        finish = true;
                    }




                });
            }
            else if (type == RequestType.PLAY_LIST)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //PlaylistVideoListRendererContent ErrorItem = new PlaylistVideoListRendererContent();
                        //ErrorItem.PlaylistVideoRenderer = new PlaylistVideoRenderer();
                        //ErrorItem.PlaylistVideoRenderer.VideoId = "TEST";


                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/playlist?list=" + query);
                        //req.UserAgent = USER_AGENT;



                        try
                        {
                            var html = source;

                            //checkHtml = html;

                            html = parseHtml(html, query);

                            if (html != string.Empty)
                            {
                                var obj = JsonConvert.DeserializeObject<Root>(html);

                                var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[0].TabRenderer.Content.SectionListRenderer.Contents[0].ItemSectionRenderer.Contents[0].PlaylistVideoListRenderer.Contents;

                                foreach (var item in list)
                                {
                                    //ErrorItem = item;
                                    // parse to obj
                                    //if (item.VideoRenderer != null)
                                    //{

                                    if (item.PlaylistVideoRenderer != null)
                                    {
                                        if (item.PlaylistVideoRenderer.LengthText != null)
                                        {
                                            if (item.PlaylistVideoRenderer.ShortBylineText != null)
                                            {
                                                if (item.PlaylistVideoRenderer.ShortBylineText.Runs != null)
                                                {
                                                    //var time = item.PlaylistVideoRenderer.LengthText.SimpleText;
                                                    //if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                    //{

                                                    //}
                                                    //else
                                                    //{
                                                    var trackId = item.PlaylistVideoRenderer.VideoId;
                                                    var title = item.PlaylistVideoRenderer.Title.Runs[0].Text;
                                                    if (title == null)
                                                    {
                                                        title = item.PlaylistVideoRenderer.Title.Runs[0].Text;
                                                        if (title == null)
                                                        {
                                                            title = item.PlaylistVideoRenderer.Title.Accessibility.AccessibilityDataAccessibilityData.Label;
                                                        }
                                                    }


                                                    //if (time.IndexOf(":") < 0)
                                                    //{
                                                    //    time = "00:00:" + time;
                                                    //}
                                                    //else if (time.Split(":").Length == 2)
                                                    //{
                                                    //    time = "00:" + time;
                                                    //}



                                                    //var duration = (long)TimeSpan.Parse(time).TotalSeconds;

                                                    var duration = item.PlaylistVideoRenderer.LengthSeconds;

                                                    //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                    var artist = item.PlaylistVideoRenderer.ShortBylineText.Runs[0].Text;

                                                    var listImg = item.PlaylistVideoRenderer.Thumbnail.Thumbnails;
                                                    var image = listImg[listImg.Count() - 1].Url;


                                                    songs.Add(new SongModel()
                                                    {
                                                        TrackId = trackId,
                                                        Duration = duration,
                                                        Title = title,
                                                        //CreateAt = creatAt,
                                                        Artist = artist,
                                                        Image = image

                                                    });
                                                    //}

                                                }

                                            }

                                        }
                                    }

                                    //}
                                }
                            }






                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET PLAY LIST API";

                            var msg = "";

                            msg = "Detail: " + e.Message + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }
                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.SEARCH_PLAY_LIST)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        if (query.IndexOf("#") == 0)
                        {
                            query = query.Replace("#", "");
                        }

                        query = query.Trim().ToLower();
                        // add search history
                        var keyWord = _context.SearchHistories.Where(x => x.KeyWord == query).FirstOrDefault();
                        if (keyWord != null)
                        {
                            var repeat = keyWord.Repeat;
                            repeat += 1;
                            keyWord.Repeat = repeat;
                        }
                        else
                        {
                            _context.SearchHistories.Add(new SearchHistory()
                            {
                                KeyWord = query,
                                Repeat = 1
                            });
                        }

                        // save
                        _context.SaveChanges();







                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/results?search_query=" + query + "&sp=EgIQAw%253D%253D");
                        //req.UserAgent = USER_AGENT;

                        try
                        {
                            var html = source;

                            html = parseHtml(html, query);

                            if (html != string.Empty)
                            {

                                checkHtml = html;

                                line = "obj";

                                var obj = JsonConvert.DeserializeObject<Models.PlayList.Temperatures>(html);

                                var itemRender = obj.Contents.TwoColumnSearchResultsRenderer.PrimaryContents.SectionListRenderer1;

                                if (itemRender == null)
                                {
                                    itemRender = obj.Contents.TwoColumnSearchResultsRenderer.PrimaryContents.SectionListRenderer2;
                                }

                                line = "temp";

                                var temp = itemRender.Contents;

                                line = "list";

                                //var list = temp[0].ItemSectionRenderer1.Contents;


                                //foreach (var item in temp)
                                //{
                                //    if (item.ItemSectionRenderer1 != null)
                                //    {
                                //        if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                //        {
                                //            list = item.ItemSectionRenderer1.Contents;
                                //            break;
                                //        }
                                //    }
                                //}



                                var itemListRender = temp[0].ItemSectionRenderer1;
                                var list = new List<Models.PlayList.ItemSectionRendererContent>();


                                if (itemListRender == null)
                                {
                                    itemListRender = temp[0].ItemSectionRenderer2;

                                    foreach (var item in temp)
                                    {
                                        if (item.ItemSectionRenderer2 != null)
                                        {
                                            if (item.ItemSectionRenderer2.Contents.Count() > 10)
                                            {
                                                list = item.ItemSectionRenderer2.Contents;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in temp)
                                    {
                                        if (item.ItemSectionRenderer1 != null)
                                        {
                                            if (item.ItemSectionRenderer1.Contents.Count() > 10)
                                            {
                                                list = item.ItemSectionRenderer1.Contents;
                                                break;
                                            }
                                        }
                                    }
                                }



                                foreach (var item in list)
                                {

                                    //currentItem = item;
                                    // parse to obj
                                    if (item.PlaylistRenderer != null)
                                    {
                                        if (item.PlaylistRenderer.PlaylistId != null)
                                        {
                                            if (item.PlaylistRenderer.ShortBylineText.Runs != null)
                                            {
                                                var trackId = item.PlaylistRenderer.PlaylistId;
                                                var title = item.PlaylistRenderer.Title.SimpleText;



                                                //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                var artist = item.PlaylistRenderer.ShortBylineText.Runs[0].Text;

                                                var listImg = item.PlaylistRenderer.Thumbnails;
                                                var image = listImg[listImg.Count() - 1].Thumbnails[0].Url;

                                                var videoCount = item.PlaylistRenderer.VideoCount;


                                                songs.Add(new SongModel()
                                                {
                                                    TrackId = trackId,
                                                    //Duration = duration,
                                                    Title = title,
                                                    //CreateAt = creatAt,
                                                    Artist = artist,
                                                    Image = image,
                                                    VideoCount = videoCount

                                                });
                                            }

                                        }

                                    }

                                    else
                                    {
                                        if (item.RichRenderer != null)
                                        {
                                            var smallItem = item.RichRenderer.Content.PlaylistRenderer;

                                            if (smallItem != null)
                                            {
                                                if (smallItem.PlaylistId != null)
                                                {
                                                    if (smallItem.ShortBylineText.Runs != null)
                                                    {
                                                        var trackId = smallItem.PlaylistId;
                                                        var title = smallItem.Title.SimpleText;



                                                        //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                        var artist = smallItem.ShortBylineText.Runs[0].Text;

                                                        var listImg = smallItem.Thumbnails;
                                                        var image = listImg[listImg.Count() - 1].Thumbnails[0].Url;

                                                        var videoCount = smallItem.VideoCount;


                                                        songs.Add(new SongModel()
                                                        {
                                                            TrackId = trackId,
                                                            //Duration = duration,
                                                            Title = title,
                                                            //CreateAt = creatAt,
                                                            Artist = artist,
                                                            Image = image,
                                                            VideoCount = videoCount

                                                        });
                                                    }

                                                }

                                            }
                                        }
                                    }


                                }
                            }






                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;

                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on SEARCH PLAY LIST API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);





                            ///////////////
                            ///


                            //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                            //{
                            //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            //}

                            //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                            //if (!Directory.Exists(folder))
                            //{
                            //    Directory.CreateDirectory(folder);
                            //}

                            //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                            //var fileName = Path.Combine(folder, nameFile);


                            //new FileWriter().WriteData(checkHtml, fileName);

                            //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                            //var subject = "[MusiNow Report] Bug on parse HTML";


                            //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);



                            // send mail to Hieu
                            //var subject = "[MusiNow Report] Bug on Bug on SEARCH PLAY LIST API";
                            //var msg = "Detail: " + e.Message + "<br/> Error Item: " + line + "<br/>" + JsonConvert.SerializeObject(currentItem) + "<br/>" + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            ////var msg = "Detail: " + e.Message + "<br/> Error Item: " + JsonConvert.SerializeObject(currentItem) + "<br/> Query Search: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //if (e.GetType() == typeof(WebException))
                            //{
                            //    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                            //    msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            //}

                            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                            checkHtml = "";

                        }
                    }
                    else
                    {
                        finish = true;
                    }




                });
            }
            else if (type == RequestType.TRENDING)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        //link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());


                        try
                        {
                            var html = source;

                            html = parseHtml(html, query);

                            if (html != string.Empty)
                            {
                                var obj = JsonConvert.DeserializeObject<Models.Trending.TrendingModel>(html);

                                var list = obj.Contents.TwoColumnBrowseResultsRenderer.Tabs[1].TabRenderer.Content.SectionListRenderer.Contents[0].ItemSectionRenderer.Contents[0].ShelfRenderer.Content.ExpandedShelfContentsRenderer.Items;

                                foreach (var item in list)
                                {
                                    var tmp = item;
                                    // parse to obj
                                    //if (item.VideoRenderer != null)
                                    //{
                                    if (tmp.VideoRenderer.LengthText != null)
                                    {
                                        if (tmp.VideoRenderer.ShortBylineText != null)
                                        {
                                            if (tmp.VideoRenderer.ShortBylineText.Runs != null)
                                            {
                                                var time = tmp.VideoRenderer.LengthText.SimpleText;
                                                time = time.Replace(".", ":");
                                                if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                {

                                                }
                                                else
                                                {
                                                    var trackId = tmp.VideoRenderer.VideoId;
                                                    var title = tmp.VideoRenderer.Title.Runs[0].Text;
                                                    if (title == null)
                                                    {
                                                        title = tmp.VideoRenderer.Title.Accessibility.AccessibilityData.Label;
                                                    }


                                                    if (time.IndexOf(":") < 0)
                                                    {
                                                        time = "00:00:" + time;
                                                    }
                                                    else if (time.Split(":").Length == 2)
                                                    {
                                                        time = "00:" + time;
                                                    }



                                                    var duration = (long)TimeSpan.Parse(time).TotalSeconds;
                                                    //var duration = item.VideoRenderer.;

                                                    //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                    var artist = tmp.VideoRenderer.ShortBylineText.Runs[0].Text;

                                                    var listImg = tmp.VideoRenderer.Thumbnail.Thumbnails;
                                                    var image = listImg[listImg.Count() - 1].Url;


                                                    songs.Add(new SongModel()
                                                    {
                                                        TrackId = trackId,
                                                        Duration = duration,
                                                        Title = title,
                                                        //CreateAt = creatAt,
                                                        Artist = artist,
                                                        Image = image

                                                    });
                                                }

                                            }

                                        }

                                    }

                                    //}
                                }
                            }







                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;



                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET TRENDING LIST API";
                            var msg = "";
                            msg = "Detail: " + e.Message + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.RELATED)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        //link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());

                        //if (link != null && link.Length != 0)
                        //{



                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=" + query);
                        //req.UserAgent = USER_AGENT;

                        try
                        {
                            var html = source;

                            html = parseHtml(html, query);

                            if (html != string.Empty)
                            {
                                var obj = JsonConvert.DeserializeObject<Models.Related.JsonModelRelated>(html);

                                var list = obj.Contents.TwoColumnWatchNextResults.SecondaryResults.SecondaryResults.Results;

                                foreach (var item in list)
                                {
                                    if (item.CompactVideoRenderer != null)
                                    {
                                        if (item.CompactVideoRenderer.VideoId != null)
                                        {
                                            if (item.CompactVideoRenderer.LengthText != null)
                                            {
                                                if (item.CompactVideoRenderer.ShortBylineText != null)
                                                {
                                                    if (item.CompactVideoRenderer.ShortBylineText.Runs != null)
                                                    {
                                                        var time = item.CompactVideoRenderer.LengthText.SimpleText;
                                                        time = time.Replace(".", ":");
                                                        if (time.Split(":").Length == 3 && int.Parse(time.Split(":")[0]) > 23)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            var trackId = item.CompactVideoRenderer.VideoId;
                                                            var title = item.CompactVideoRenderer.Title.SimpleText;
                                                            if (title == null)
                                                            {
                                                                title = item.CompactVideoRenderer.Title.Accessibility.AccessibilityData.Label;
                                                            }


                                                            if (time.IndexOf(":") < 0)
                                                            {
                                                                time = "00:00:" + time;
                                                            }
                                                            else if (time.Split(":").Length == 2)
                                                            {
                                                                time = "00:" + time;
                                                            }



                                                            var duration = (long)TimeSpan.Parse(time).TotalSeconds;
                                                            //var duration = item.VideoRenderer.;

                                                            //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                                            var artist = item.CompactVideoRenderer.ShortBylineText.Runs[0].Text;

                                                            var listImg = item.CompactVideoRenderer.Thumbnail.Thumbnails;
                                                            var image = listImg[listImg.Count() - 1].Url;


                                                            songs.Add(new SongModel()
                                                            {
                                                                TrackId = trackId,
                                                                Duration = duration,
                                                                Title = title,
                                                                //CreateAt = creatAt,
                                                                Artist = artist,
                                                                Image = image

                                                            });
                                                        }

                                                    }

                                                }

                                            }
                                        }
                                    }
                                    // parse to obj
                                    //if (item.VideoRenderer != null)
                                    //{


                                    //}
                                }
                            }







                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;



                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET Related LIST API";
                            var msg = "";
                            msg = "Detail: " + e.Message + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }
                        //}
                        //else
                        //{
                        //    finish = true;
                        //}


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }
            else if (type == RequestType.VIDEO)
            {
                queue.QueueBackgroundWorkItem(async token =>
                {
                    // check KEY
                    if (key == KEY)
                    {
                        //string link = "";

                        //if (query.Trim().ToUpper() == TrendingCountryCode.US.ToString())
                        //{

                        //}
                        //else if (query.Trim().ToUpper() == TrendingCountryCode.UK.ToString())
                        //{
                        //    link = this.configuration.GetConnectionString("MailReceiver"); 
                        //}
                        //link = configuration.GetConnectionString("Trending_" + query.Trim().ToUpper());

                        //if (link != null && link.Length != 0)
                        //{



                        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=" + query);
                        //req.UserAgent = USER_AGENT;

                        try
                        {
                            var html = source;

                            html = parseHtmlVideo(html, query);

                            if (html != string.Empty)
                            {
                                var obj = JsonConvert.DeserializeObject<Models.JsonModelTrack>(html);

                                //var list = obj.Contents.TwoColumnWatchNextResults.SecondaryResults.SecondaryResults.Results;
                                var item = obj.VideoDetails;

                                var trackId = query;
                                var title = item.Title;






                                var duration = item.LengthSeconds;
                                //var duration = item.VideoRenderer.;

                                //var creatAt = item.VideoRenderer.PublishedTimeText.SimpleText;
                                var artist = item.Author;

                                var listImg = item.Thumbnail.Thumbnails;
                                var image = listImg[listImg.Count() - 1].Url;


                                songs.Add(new SongModel()
                                {
                                    TrackId = trackId,
                                    Duration = duration,
                                    Title = title,
                                    //CreateAt = creatAt,
                                    Artist = artist,
                                    Image = image

                                });
                            }






                            finish = true;
                        }
                        catch (Exception e)
                        {
                            finish = true;



                            // send mail to Hieu
                            var subject = "[MusiNow Report] Bug on GET VIDEO TRACK API";
                            var msg = "";
                            msg = "Detail: " + e.Message + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            if (e.GetType() == typeof(WebException))
                            {
                                string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
                                msg = "Detail: " + error + "<br/> Video ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                            }

                            MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                        }
                        //}
                        //else
                        //{
                        //    finish = true;
                        //}


                    }
                    else
                    {
                        finish = true;
                    }
                });
            }



            // 
            var end = DateTime.Now.AddMinutes(TIME_OUT);


            while (true)
            {
                if (DateTime.Compare(end, DateTime.Now) <= 0)
                {
                    break;
                }

                Thread.Sleep(100);
                if (finish)
                {
                    break;
                }
            }

            //var subject = "[MusiNow Report] Bug on SEARCH PLAY LIST API";
            //var msg = "<h3>Detail:</h3> <br/> Query Search: " + query + "<br/>" + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);
            //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, "anhlh@digitoy.xyz", subject, msg);

            // put processing code here
            //return checkHtml;

            return JsonConvert.SerializeObject(songs);






        }


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        //[HttpGet("link")]
        //public async Task<string> Get([FromHeader] string key, string id)
        //{
        //    bool finish = false;
        //    string link = "";



        //    queue.QueueBackgroundWorkItem(async token =>
        //    {
        //        // check KEY
        //        if (key != KEY)
        //        {
        //            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.youtube.com/watch?v=" + id);
        //            req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/600.8.9 (KHTML, like Gecko) Version/8.0.8 Safari/600.8.9";

        //            try
        //            {
        //                using (var resp = req.GetResponse())
        //                {
        //                    var html = await new StreamReader(resp.GetResponseStream()).ReadToEndAsync();

        //                    link = "{" + searchRegex(html) + "}";

        //                    //link = html;
        //                    var obj = JsonConvert.DeserializeObject<LinkModel>(link);

        //                    link = obj.Link;



        //                    finish = true;
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                finish = true;

        //                //send mail to Hieu
        //                var subject = "[MusiNow Report] Bug on get LINK API";
        //                var msg = "Detail: " + e.Message + "<br/> ID: " + id + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
        //                if (e.GetType() == typeof(WebException))
        //                {
        //                    string error = new StreamReader(((WebException)e).Response.GetResponseStream()).ReadToEnd().ToString();
        //                    msg = "Detail: " + error + "<br/> ID: " + id + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
        //                }

        //                MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

        //            }
        //        }
        //        else
        //        {
        //            finish = true;
        //        }




        //    });


        //    var end = DateTime.Now.AddMinutes(TIME_OUT);


        //    while (true)
        //    {
        //        if (DateTime.Compare(end, DateTime.Now) <= 0)
        //        {
        //            break;
        //        }

        //        Thread.Sleep(100);
        //        if (finish)
        //        {
        //            break;
        //        }
        //    }

        //    //var subject = "[MusiNow Report] Bug on SEARCH PLAY LIST API";
        //    //var msg = "<h3>Detail:</h3> <br/> Query Search: " + query + "<br/>" + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
        //    //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);
        //    //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, "anhlh@digitoy.xyz", subject, msg);

        //    // put processing code here
        //    return JsonConvert.SerializeObject(link);

        //}

        [HttpGet("config")]
        public async Task<string> Get([FromHeader] string key)
        {
            if (key == KEY)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    {
                        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Config");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileName = Path.Combine(folder, "config.co");

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName))
                        {

                        }
                    }

                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        string json = reader.ReadToEnd();

                        return json;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }

        [HttpGet("playlistupdated")]
        public async Task<string> GetPlayListNew([FromHeader] string key)
        {
            if (key == KEY)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    {
                        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Config");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileName = Path.Combine(folder, "playlistupdated.json");

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName))
                        {

                        }
                    }

                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        string json = reader.ReadToEnd();

                        return json;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }

        [HttpGet("playlistwithads")]
        public async Task<string> GetPlayListWithAds([FromHeader] string key)
        {
            if (key == KEY)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    {
                        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Config");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileName = Path.Combine(folder, "playlistwithads.json");

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName))
                        {

                        }
                    }

                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        string json = reader.ReadToEnd();

                        return json;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }

        [HttpGet("playlist")]
        public async Task<string> GetPlayList([FromHeader] string key)
        {
            if (key == KEY)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    {
                        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Config");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileName = Path.Combine(folder, "playlist.json");

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName))
                        {

                        }
                    }

                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        string json = reader.ReadToEnd();

                        return json;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }

        [HttpGet("ads-config")]
        public async Task<string> GetAds([FromHeader] string key)
        {
            if (key == KEY)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    {
                        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var folder = Path.Combine(environment.WebRootPath, "Config");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileName = Path.Combine(folder, "ads-config.json");

                    if (!System.IO.File.Exists(fileName))
                    {
                        using (System.IO.File.Create(fileName))
                        {

                        }
                    }

                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        string json = reader.ReadToEnd();

                        return json;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }



        private string searchRegex(string str)
        {
            //string temp = "\\\\" + "\"" + "hlsManifestUrl" + "\\\\" + "\"" + "(((?!" + "\\\\" + "\"" + ").)*)" + "\\\\" + "\"";

            str = str.Replace("\\", "");

            string pattern = string.Format("\"hlsManifestUrl\":\"(((?!\").)*)\"");

            Regex rgx = new Regex(pattern);

            var result = rgx.Matches(str);
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return result[0].Value;
                }
            }

            return "";
        }

        private string parseHtmlTest(string html, string query)
        {
            if (string.IsNullOrWhiteSpace(environment.WebRootPath))
            {
                environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var folder1 = Path.Combine(environment.WebRootPath, "Bugs");
            if (!Directory.Exists(folder1))
            {
                Directory.CreateDirectory(folder1);
            }

            var nameFile1 = "index.html";
            var fileName1 = Path.Combine(folder1, nameFile1);



            html = new FileWriter().ReadData(fileName1);
            return html;
        }
        private string parseHtml(string html, string query)
        {
            //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
            //{
            //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            //}

            //var folder1 = Path.Combine(environment.WebRootPath, "Bugs");
            //if (!Directory.Exists(folder1))
            //{
            //    Directory.CreateDirectory(folder1);
            //}

            //var nameFile1 = "index.html";
            //var fileName1 = Path.Combine(folder1, nameFile1);



            //html = new FileWriter().ReadData(fileName1);

            // old version
            int start = html.IndexOf("window[\"ytInitialData\"] = ");
            int end = html.IndexOf("window[\"ytInitialPlayerResponse\"]");

            if (start >= 0 && end >= 0 && start < end)
            {
                html = html.Substring(start, end - start);

                html = html.Replace("window[\"ytInitialData\"] = ", "").Trim();

                html = html.Substring(0, html.Length - 1);

            }
            else
            {
                start = html.IndexOf("ytInitialData = ");
                end = html.IndexOf("// scraper_data_end");

                if (start >= 0 && end >= 0 && start < end)
                {
                    html = html.Substring(start, end - start);

                    html = html.Replace("ytInitialData = ", "").Trim();

                    html = html.Substring(0, html.Length - 1);
                }
                else
                {
                    start = html.IndexOf("ytInitialData = ");
                    end = html.IndexOf("</script><title>");

                    if (start >= 0 && end >= 0 && start < end)
                    {
                        html = html.Substring(start, end - start);

                        html = html.Replace("ytInitialData = ", "").Trim();

                        html = html.Substring(0, html.Length - 1);
                    }
                    else
                    {
                        start = html.IndexOf("ytInitialData = ");
                        end = html.IndexOf("</script><script nonce=\"6ggbINoYPmfsFOEywQj7Vw\" >if (window.ytcsi) {window.ytcsi.tick('pdr', null, '');}</script>");

                        string regex = "</script><script nonce=\"[a-zA-Z0-9+-?]*\">if \\(window.ytcsi\\) \\{window.ytcsi.tick\\('pdr', null, ''\\);\\}</script>";

                        Match match = Regex.Match(html, regex);
                        if (match.Success)
                        {
                            end = match.Index;
                        }

                        if (start >= 0 && end >= 0 && start < end)
                        {
                            html = html.Substring(start, end - start);

                            html = html.Replace("ytInitialData = ", "").Trim();

                            html = html.Substring(0, html.Length - 1);
                        }
                        else
                        {
                            start = html.IndexOf("ytInitialData = ");
                            end = html.IndexOf("</script><link rel=\"alternate\" media=\"handheld\"");


                            if (start >= 0 && end >= 0 && start < end)
                            {
                                html = html.Substring(start, end - start);

                                html = html.Replace("ytInitialData = ", "").Trim();

                                html = html.Substring(0, html.Length - 1);
                            }
                            else
                            {
                                //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                                //{
                                //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                                //}

                                //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                                //if (!Directory.Exists(folder))
                                //{
                                //    Directory.CreateDirectory(folder);
                                //}

                                //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                                //var fileName = Path.Combine(folder, nameFile);


                                //new FileWriter().WriteData(html, fileName);

                                //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                                //var subject = "[MusiNow Report] Bug on parse HTML";


                                //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> Play List ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                                //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                                html = "";
                            }
                        }








                    }


                }



            }

            return html;
        }


        private string parseHtmlVideo(string html, string query)
        {
            int start = html.IndexOf("ytInitialPlayerResponse = ");
            int end = html.IndexOf("var meta = document.createElement");

            if (start >= 0 && end >= 0 && start < end)
            {
                html = html.Substring(start, end - start);

                html = html.Replace("ytInitialPlayerResponse = ", "").Trim();

                html = html.Substring(0, html.Length - 1);
            }
            else
            {
                 start = html.IndexOf("ytInitialPlayerResponse = ");
                 end = html.IndexOf("</script><div id=\"player\"");

                if (start >= 0 && end >= 0 && start < end)
                {
                    html = html.Substring(start, end - start);

                    html = html.Replace("ytInitialPlayerResponse = ", "").Trim();

                    html = html.Substring(0, html.Length - 1);
                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(environment.WebRootPath))
                    //{
                    //    environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    //}

                    //var folder = Path.Combine(environment.WebRootPath, "Bugs");
                    //if (!Directory.Exists(folder))
                    //{
                    //    Directory.CreateDirectory(folder);
                    //}

                    //var nameFile = "bug-playlist-" + DateTime.Now.ToFileTime() + ".html";
                    //var fileName = Path.Combine(folder, nameFile);


                    //new FileWriter().WriteData(html, fileName);

                    //string ip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");

                    //var subject = "[MusiNow Report] Bug on parse HTML VIDEO";


                    //var msg = "Detail: " + ip + ":7071/bugs/" + nameFile + "<br/> VIDEO ID: " + query + "<br/>Time: " + DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToLongDateString();
                    //MailHelper.sendMail(MAIL_SENDER, PW_MAIL_SENDER, MAIL_RECEIVER, subject, msg);

                    html = "";
                }

                   
            }






            return html;
        }


    }
}
