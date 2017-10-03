using ShauliBlogProject.DAL;
using ShauliBlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Microsoft.AspNet.Identity;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace ShauliBlogProject.Controllers
{
    public class BlogController : Controller
    {
        private ApplicationDbContext dbUsers = new ApplicationDbContext();
        private ShauliBlogContext db = new ShauliBlogContext();
        private const int PostPerPage = 4;

        public ActionResult Index(int? id)
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                int i = db.Posts.Count();
                double[][] inputs = new double[i][];
                int[] outputs = new int[i];
                int j = 0;

                var types = db.UserTypes.Where(x => x.UserID == userId);

                foreach (var n in types)
                {
                    var ID = n.PostID;
                    PostType type = db.Types.Where(x => x.PostTypeID == ID).First();

                    inputs[j] = new double[5];

                    //inputs[j] = type.getTypeArray();
                    inputs[j][0] = type.Sport;
                    inputs[j][1] = type.Music;
                    inputs[j][2] = type.Food;
                    inputs[j][3] = type.Vacation;
                    inputs[j][4] = type.Study;

                    outputs[j] = n.Visit;
                    j++;
                }

                var teacher = new SequentialMinimalOptimization<Gaussian>()
                {
                    UseComplexityHeuristic = true,
                    UseKernelEstimation = true // estimate the kernel from the data
                };

                // Teach the vector machine
                var svm = teacher.Learn(inputs, outputs);
                //Get all posts
                var _posts = (from p in db.Posts select p);
                //New list for login customer
                List<Post> SVMPosts = new List<Post>();
                foreach (var p in _posts)
                {                    
                    bool answer = svm.Decide(p.Types.getTypeArray());
                    if (answer)
                    {
                        SVMPosts.Insert(0, p);
                    } else
                    {
                        SVMPosts.Add(p);
                    }
                }
                return View(SVMPosts);
            }

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
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var types = db.UserTypes.Where(x => x.UserID == userId);
                UserType userType = types.Where(x => x.PostID == id).First();
                userType.Visit = 1;
                db.SaveChanges();
            }

            Post post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        [Authorize(Roles = "Admins")]
        [ValidateInput(false)]
        public ActionResult Update(int? PostID, string Title, string Author, string url_author_post, DateTime postDate, string contentPost, string image, string video, int food = 0, int sport = 0, int vacation = 0, int study = 0, int music = 0)
        {
            if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Author) && !String.IsNullOrEmpty(url_author_post) && !String.IsNullOrEmpty(contentPost))
            {
                Post post = GetPost(PostID);
                post.Title = Title;
                post.Author = Author;
                post.url_author_post = url_author_post;
                post.postDate = postDate;
                post.contentPost = contentPost;
                post.image = image;
                post.video = video;

                PostType type;
                if (!PostID.HasValue)
                {
                    type = new PostType();
                }
                else
                {
                    type = db.Types.Where(x => x.PostTypeID == PostID).First();
                }

                type.Food = food;
                type.Sport = sport;
                type.Vacation = vacation;
                type.Study = study;
                type.Music = music;
                type.Post = post;
                post.Types = type;

                if (!PostID.HasValue)
                {
                    db.Posts.Add(post);
                    db.Types.Add(type);
                }

                try
                {
                    PostToWall(post, 10205420130967215, "EAACEdEose0cBAIGOHy0TQOLDzWvpFK5CzEieFmMwAax4imOWP9UtSAmNpdwolcYWnDG4Nb9RVcIRUzoAIHET9ZCaB4hN4CPxFi42jnEnYWvjZBxROFtYw9GJXTZC1bzXehTaOXGuGx6ZBPvW6XnZBd0dngTImn4tTYwfWDWyFrjrMEjYXSYRoyNegzFmZCEZCcZD");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                db.SaveChanges();

                if (!PostID.HasValue)
                {
                    foreach (var u in dbUsers.Users)
                    {
                        UserType userType = new UserType();
                        userType.UserID = u.Id;
                        userType.PostID = post.PostID;
                        userType.Visit = 0;
                        db.UserTypes.Add(userType);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = post.PostID });
            }
            return RedirectToAction("Edit", new { id = PostID });
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
            if (id.HasValue)
            {
                PostType type = db.Types.Where(x => x.PostTypeID == id).First();
                ViewBag.vFood = Convert.ToBoolean(type.Food);
                ViewBag.vSport = Convert.ToBoolean(type.Sport);
                ViewBag.vVacation = Convert.ToBoolean(type.Vacation);
                ViewBag.vStudy = Convert.ToBoolean(type.Study);
                ViewBag.vMusic = Convert.ToBoolean(type.Music);
            }
            return View(post);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult Delete(int id)
        {
            if (User.IsInRole("Admins"))
            {
                IQueryable<UserType> userTypes = db.UserTypes.Where(x => x.PostID == id);
                foreach(var userType in userTypes)
                {
                    db.UserTypes.Remove(userType);
                }
                PostType postType = db.Types.Where(x => x.PostTypeID == id).First();
                db.Types.Remove(postType);
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
            if(db.SeggPosts.Count() == 0)
            {
                return null;
            }
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