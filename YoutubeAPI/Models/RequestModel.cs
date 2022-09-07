using System;
using Newtonsoft.Json;

namespace YoutubeAPI.Models
{
    public class RequestSearchModel
    {
        public string Key { get; set; }
        public string QuerySearch { get; set; }
    }

    public class RequestListModel
    {
        public string Key { get; set; }
        public string PlayListId { get; set; }
    }

    public class LinkModel
    {
        [JsonProperty("hlsManifestUrl")]
        public string Link { get; set; }
    }
}
