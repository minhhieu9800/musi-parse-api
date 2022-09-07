namespace YoutubeAPI.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class JsonModelTrack
    {
        [JsonProperty("videoDetails")]
        public VideoDetails VideoDetails { get; set; }
    }

    public partial class VideoDetails
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("lengthSeconds")]
    
        public long LengthSeconds { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("isOwnerViewing")]
        public bool IsOwnerViewing { get; set; }

        [JsonProperty("isCrawlable")]
        public bool IsCrawlable { get; set; }

        [JsonProperty("thumbnail")]
        public VideoDetailsThumbnail Thumbnail { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("allowRatings")]
        public bool AllowRatings { get; set; }

        [JsonProperty("viewCount")]
       
        public long ViewCount { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("isUnpluggedCorpus")]
        public bool IsUnpluggedCorpus { get; set; }

        [JsonProperty("isLiveContent")]
        public bool IsLiveContent { get; set; }
    }

    public partial class VideoDetailsThumbnail
    {
        [JsonProperty("thumbnails")]
        public List<ThumbnailElement> Thumbnails { get; set; }
    }

    public partial class ThumbnailElement
    {
        //[JsonProperty("url")]
        //public string Url { get; set; }

        //[JsonProperty("width")]
        //public long Width { get; set; }

        //[JsonProperty("height")]
        //public long Height { get; set; }
    }
}
