using System;
using System.Collections.Generic;

#nullable disable

namespace YoutubeAPI.Models
{
    public partial class SearchHistory
    {
        public int Id { get; set; }
        public string KeyWord { get; set; }
        public int? Repeat { get; set; }
    }
}
