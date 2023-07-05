using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeSearch.Models;

namespace YoutubeSearch.Controllers
{
    public class HomeController : Controller
    {
        //Khai báo một hằng số chứa API key của ứng dụng.
        private const string API_KEY = "AIzaSyC1OYVXieXCRLANzmTbqBshO5WSRh_vf78";

        //Action Index() lấy query từ người dùng, tạo một đối tượng YouTubeService với API key,
        //gửi yêu cầu tìm kiếm video với query và trả về kết quả dưới dạng List các đối tượng VideoModel.
        public ActionResult Index(string query)
        {
            // Khởi tạo dịch vụ YouTubeService
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = API_KEY,
                ApplicationName = this.GetType().ToString()
            });
            // Tạo request để lấy danh sách kết quả tìm kiếm trên YouTube
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.MaxResults = 20;

            // Thực hiện request và lấy danh sách kết quả trả về
            var searchListResponse = searchListRequest.Execute();
            var videos = new List<VideoModel>();

            // Lặp qua từng kết quả tìm kiếm trả về để lấy thông tin video
            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind == "youtube#video")
                {
                    // Tạo một đối tượng VideoModel từ thông tin video
                    videos.Add(new VideoModel()
                    {
                        Id = searchResult.Id.VideoId,
                        Title = searchResult.Snippet.Title,
                        Description = searchResult.Snippet.Description,
                        ThumbnailUrl = searchResult.Snippet.Thumbnails.Default__.Url,
                        VideoUrl = $"https://www.youtube.com/watch?v={searchResult.Id.VideoId}"
                    });
                }
            }
            // Trả về trang chủ và danh sách video tìm kiếm được

            return View(videos);
        }

        // Action trả về trang phát video và các bình luận của video đó
        public ActionResult Player(string id)
        {
            // Khởi tạo dịch vụ YouTubeService
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = API_KEY,
                ApplicationName = this.GetType().ToString()
            });
            // Tạo request để lấy thông tin video từ ID của video đó
            var videoListRequest = youtubeService.Videos.List("snippet,statistics");
            videoListRequest.Id = id;

            // Thực hiện request và lấy thông tin video trả về
            var videoListResponse = videoListRequest.Execute();
            // Tạo một đối tượng VideoModel từ thông tin video
            var video = new VideoModel()
            {
                Id = videoListResponse.Items[0].Id,
                Title = videoListResponse.Items[0].Snippet.Title,
                Description = videoListResponse.Items[0].Snippet.Description,
                ThumbnailUrl = videoListResponse.Items[0].Snippet.Thumbnails.Default__.Url,
                VideoUrl = $"https://www.youtube.com/watch?v={id}",
                ViewCount = (ulong)videoListResponse.Items[0].Statistics.ViewCount,
                LikeCount = (ulong)videoListResponse.Items[0].Statistics.LikeCount,
                PublishedAt = videoListResponse.Items[0].Snippet.PublishedAt.GetValueOrDefault()
               
            };


            // Tạo request để lấy thông tin bình luận video từ ID của video đó
            var commentThreadRequest = youtubeService.CommentThreads.List("snippet");
            commentThreadRequest.VideoId = id;
            var commentThreadResponse = commentThreadRequest.Execute();

            // Ánh xạ luồng bình luận vào  đối tượng CommentModel 
            var comments = commentThreadResponse.Items.Select(commentThread =>
            {
                var comment = commentThread.Snippet.TopLevelComment.Snippet;
                return new CommentModel
                {
                    Author = comment.AuthorDisplayName,
                    Text = comment.TextDisplay,
                    PublishedAt = comment.PublishedAt.GetValueOrDefault(),
                    LikeCount = (ulong)comment.LikeCount,
                    AuthorProfileImageUrl = comment.AuthorProfileImageUrl

                };
            }).ToList();

            // Truyền thông tin video và bình luận của video vào đối tượng playerViewModel
            var playerViewModel = new PlayerViewModel
            {
                Video = video,
                Comments = comments
            };
            //Truyền đối tượng playerViewModel qua View 
            return View(playerViewModel);

           
        }



    }

}
