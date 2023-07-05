using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeSearch.Models
{
    public class CommentModel
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime PublishedAt { get; set; }
        public ulong LikeCount { get; set; }
        public string AuthorProfileImageUrl { get; set; }
        
    }
}