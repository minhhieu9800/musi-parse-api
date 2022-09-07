using System;
namespace YoutubeAPI.Models
{
    public class SongModel
    {
        public string TrackId { get; set; }
        public long Duration { get; set; }
        public DateTime CreateAt { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Image { get; set; }

        public long VideoCount { get; set; }

    }
}
