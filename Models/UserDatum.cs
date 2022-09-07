using System;
using System.Collections.Generic;

#nullable disable

namespace YoutubeAPI.Models
{
    public partial class UserDatum
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DeviceBackUp { get; set; }
        public string LastBackUp { get; set; }
        public string DataBackUp { get; set; }
    }
}
