using ShauliBlogProject.DAL;
using ShauliBlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;


namespace ShauliBlogProject.Controllers
{
    public class BlogController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();
        private const int PostPerPage = 4;

        public ActionResult Index(int? id)
        {
            int pageNumber = id ?? 0;
            IEnumerable<Post> posts =
                (from post in db.Posts
                 where post.postDate < DateTime.Now
                 orderby post.postDate descending
                 select post).Skip(pageNumber * PostPerPage).Take(PostPerPage + 1);
            ViewBag.IsPreviousLinkVisible = pageNumber > 0;
            ViewBag.IsNextLinkVisible = posts.Count() > PostPerPage;
            ViewBag.PageNumber = pageNumber;
            ViewBag.IsAdmin = IsAdmin;
            return View(posts.Take(PostPerPage));
        }

        [HttpPost]
        public ActionResult Index(int? id, string dateSearch, string authorSearch, string emailSearch, string wordsSearch)
        {
            var segg = db.SeggPosts.Where(l => l.ByAuthor == authorSearch);
            if(segg.Count() == 0)
            {
                var seg = new SeggPost { ByAuthor = authorSearch, Count = 1 };
                db.SeggPosts.Add(seg);
                db.SaveChanges();
            }
            else
            {
                segg.First().Count++;
                db.SaveChanges();
            }

            var result = (from post in db.Posts select post);

            if (!String.IsNullOrEmpty(dateSearch))
            {
                DateTime date = DateTime.Parse(dateSearch);
                result = result.Where(x => x.postDate >= date);
            }
            if (!String.IsNullOrEmpty(authorSearch))
            {
                result = result.Where(x => x.Author.Contains(authorSearch));
            }
            if (!String.IsNullOrEmpty(emailSearch))
            {
                result = result.Where(x => x.url_author_post.Contains(emailSearch));
            }
            if (!String.IsNullOrEmpty(wordsSearch))
            {
                result = result.Where(x => x.contentPost.Contains(wordsSearch));
            }

            ViewBag.IsAdmin = IsAdmin;
            return View(result);
        }

        //[Authorize(Roles = "Admins")]
        public ActionResult details(int id)
        {
            Post post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        [Authorize(Roles = "Admins")]
        [ValidateInput(false)]
        public ActionResult Update(int? PostID, string Title, string Author, string url_author_post, DateTime postDate, string contentPost, string image, string video)
        {
            Post post = GetPost(PostID);
            post.Title = Title;
            post.Author = Author;
            post.url_author_post = url_author_post;
            post.postDate = postDate;
            post.contentPost = contentPost;
            post.image = image;
            post.video = video;

            if (!PostID.HasValue)
            {
                db.Posts.Add(post);
            }
            try
            {
                PostToWall(post, 10205420130967215, "EAACEdEose0cBAPYos90mjceVq43qutdYmxlS66ZAcnYZCvgRTI1mQtcdlR2ZCcnmV8G67ZA916kQWIH7E240qMlZAjscu1cMcmtv9BJNUeX2ZCOxEdlfzZBMV5HOZBugJJFtrjb5bmLwHChRWUXgx16APB8xBZCMJIHZAA1U3ZCj1pVfYCGjKhGZC3av7ZCWBRf39lIrmzVZBNdrus8wZDZD");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = post.PostID });
        }

        [ValidateInput(false)]
        public ActionResult Comment(int id, string Title, string Author, string url_author_comment, string content_comment)
        {
            Post post = GetPost(id);
            Comment comment = new Comment();
            comment.post = post;
            comment.Title = Title;
            comment.Author = Author;
            comment.url_author_comment = url_author_comment;
            comment.content_comment = content_comment;
            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            return View(post);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admins"))
            {
                Post post = GetPost(id);
                db.Posts.Remove(post);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admins")]
        public ActionResult DeleteComment(int id)
        {
            if (User.IsInRole("Admins"))
            {
                Comment comment = db.Comments.Where(x => x.CommentID == id).First();
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private Post GetPost(int? id)
        {
            return id.HasValue ? db.Posts.Where(x => x.PostID == id).First() : new Post { PostID = -1 };
        }

        public ActionResult Plugin()
        {
            var fans = (from u in db.Comments
                        join b in db.FansClub
                        on u.url_author_comment equals b.Email
                        select u);

            var result = db.FansClub.GroupBy(i => i.Country).Select(i => new { Country = i.Key, Count = i.Count() }).ToList();
            List<FansGroubByViewModel> FansCountries = new List<FansGroubByViewModel>();
            double count = (from o in db.FansClub select o).Count();

            foreach (var x in result)
            {
                FansGroubByViewModel f = new FansGroubByViewModel(x.Country, x.Count);
                var percent = ((double)x.Count / count) * 100;
                percent = Math.Round((double)percent, 2);
                f.percent = percent;
                FansCountries.Add(f);
            }

            ViewBag.Countries = FansCountries;
            ViewBag.fans = fans;

            return View();
        }

        public ActionResult _SeggPost()
        {
            var segg = (from s in db.SeggPosts
                        orderby s.Count descending
                        select s).First();

            var post = db.Posts.Where(x => x.Author.Contains(segg.ByAuthor)).First();
            return PartialView(post);
        }

        private void PostToWall(Post message, long userId, string wallAccessToken)
        {
            var fb = new FacebookClient(wallAccessToken);
            string url = null;
            var argList = new Dictionary<string, object>();

            url = string.Format("{0}/{1}", userId, "feed");
            argList["message"] = "Post Title: " + message.Title + " \n Author: " + message.Author + "\n Post: " + message.contentPost;
            argList["link"] = message.url_author_post;
            dynamic result = fb.Post(url, argList);
            
            //String newPostId = null;
            //newPostId = result.id;            
            //message.FacebookPostID = newPostId;
        }

        //To Delete
        public bool IsAdmin
        {
            get
            {
                return Session["IsAdmin"] != null && (bool)Session["IsAdmin"];
            }
        }
    }
}