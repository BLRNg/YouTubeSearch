using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeSearch.Models
{
    public class PlayerViewModel
    {
        public VideoModel Video { get; set; }
        public List<CommentModel> Comments { get; set; }
    }

}