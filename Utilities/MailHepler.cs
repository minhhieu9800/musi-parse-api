using System;
using MimeKit;

namespace YoutubeAPI.Utilities
{
    public class MailHepler
    {
        public class MailHelper
        {
            public static void sendMail(string mailSender, string pwMailSender, string mailReceiver, string subject, string msg)
            {
                //try
                //{
                //    //var mailSender = Configuration.GetConnectionString("MailSender");
                //    //var pwMailSender = Configuration.GetConnectionString("PwMailSender");
                //    //var mailReceiver = Configuration.GetConnectionString("MailReceiver");

                //    var message = new MimeMessage();
                //    message.From.Add(new MailboxAddress("MusiNow Report", mailSender));
                //    message.To.Add(new MailboxAddress("HOST", mailReceiver));
                //    message.Subject = subject;
                //    //message.Body = new TextPart("plain")
                //    //{
                //    //    Text = emailData
                //    //};
                //    BodyBuilder bodyBuilder = new BodyBuilder();

                //    //bodyBuilder.TextBody = msg;
                //    bodyBuilder.HtmlBody = msg;



                //    message.Body = bodyBuilder.ToMessageBody();


                //    using (var client = new MailKit.Net.Smtp.SmtpClient())
                //    {

                //        client.Connect("smtp.gmail.com", 465, true);

                //        //SMTP server authentication if needed
                //        client.Authenticate(mailSender, pwMailSender);

                //        client.Send(message);

                //        client.Disconnect(true);

                //        Console.WriteLine(message);

                //        //client.Dispose();
                //    }

                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }
        }
    }
}
