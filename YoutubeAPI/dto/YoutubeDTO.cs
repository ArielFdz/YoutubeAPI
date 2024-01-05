using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Common;

namespace YoutubeAPI.dto
{
    public class YoutubeDTO
    {
        public string Url {get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}