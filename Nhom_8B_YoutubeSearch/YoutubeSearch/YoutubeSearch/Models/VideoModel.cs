using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeSearch.Models
{
    public class VideoModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }

        public DateTime PublishedAt { get; set; }
        public ulong ViewCount { get; set; }

        public ulong LikeCount { get; set; }

      


    }
}