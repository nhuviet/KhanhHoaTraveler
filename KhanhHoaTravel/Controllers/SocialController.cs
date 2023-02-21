using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace KhanhHoaTravel.Controllers
{
    public class SocialController : Controller
    {
        List<Post> Posts = new List<Post>();
        List<PostLike> PostLikes = new List<PostLike>();
        List<PostLike> listUserLikePost = new List<PostLike>();
        List<PostComment> postComments = new List<PostComment>();


        public void LikeCount(List<Post> Posts, List<PostLike> PostLikes)
        {
            foreach(var postLike in PostLikes)
            {
                foreach(var post in Posts) { 
                    if(post.Id == postLike.PostId)
                    {
                        post.LikeCount++;
                    }
                }
            }
        }
        public void CommentCount(List<Post> Posts, List<PostComment> postComments)
        {
            foreach (var poscmt in postComments)         
                foreach (var post in Posts)             
                    if (post.Id == poscmt.PostId)
                        post.CommentCount++;
        }

        public List<PostLike> ListUserLike(int Uid, List<PostLike> PostLikes)
        {
            List<PostLike> listUserLike = new List<PostLike>();
            foreach (var postLike in PostLikes)
            {
                if (postLike.Userid == Uid)
                    listUserLike.Add(postLike);
            }
            return listUserLike;
        }

        public IActionResult Index()
        {
            _User LoginUser = DataProvider.getUser(HttpContext);
            
            PostLikes = DataProvider.getPostLike();
            Posts = DataProvider.getListPost();
            postComments = DataProvider.getPostComment();
            LikeCount(Posts, PostLikes);
            CommentCount(Posts, postComments);
            if (LoginUser.Id != 0)
                listUserLikePost = ListUserLike(LoginUser.Id, PostLikes);

            ViewBag.postComments = postComments;
            ViewBag.LoginUser = LoginUser;
            ViewBag.ListPost = Posts;
            ViewBag.UserLikePost = listUserLikePost;
            //string str;
            //foreach(var p in Posts)
            //{
            //    foreach (var item in listUserLikePost)
            //    {
            //        if (item.Userid == p.User.Id)
            //        {
            //            str = "like";
            //        }
            //        else
            //            str = "unlike";
            //    }
            //}
            
            
            return View();
        }


        public void updateUserImageFile(IFormFile Image, string FileName)
        {
            if (Image != null && Image.Length > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                fileName = FileName;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Post", fileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
        }


        [HttpPost]
        public IActionResult CreatePost(Post post, IFormFile Image)
        {
            post.User = DataProvider.getUser(HttpContext);
            post.TimeModified = DateTime.Now.ToString(new CultureInfo("en-GB"));
            post.ImagePath = "";
            if(Image != null)
            {
                post.ImagePath = (DataProvider.getMaxPostId() + 1).ToString() + ".jpg";
                updateUserImageFile(Image, post.ImagePath);
            }

            DataProvider.addPost(Posts, post);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateComment(int postId, int userId, string cmt_content)
        {
            PostComment postComment = new PostComment();
            postComment.PostId = postId;
            _User user = new _User();
            user = DataProvider.getUserInfoById(userId);
            postComment.User = user;
            postComment.Content = cmt_content;

            return RedirectToAction("Index");
        }


    }
}
