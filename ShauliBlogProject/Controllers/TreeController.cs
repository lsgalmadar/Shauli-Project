using Newtonsoft.Json;
using ShauliBlogProject.DAL;
using ShauliBlogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShauliBlogProject.Controllers
{
    public class TreeController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();
        private string vjson;
        private int vid;

        public ActionResult TreeGenerate()
        {
            ViewBag.json = "Posts.xml";
            ViewBag.id = vid;

            return View();
        }

        public ActionResult PostTreeD3()
        {

            IEnumerable<SelectListItem> posts = db.Posts.Select(p => new SelectListItem { Value = p.PostID.ToString(), Text = p.Title });
            ViewBag.Posts = posts;

            return View();
        }
        [HttpPost]
        public ActionResult PostTreeD3(int PostID)
        {

            Post p = db.Posts.Where(x => x.PostID == PostID).First();
            postTree posttree = new postTree(p);

            string json = JsonConvert.SerializeObject(posttree);
            var MapPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Posts.xml");
            System.IO.File.WriteAllText(MapPath, json);

            //ViewBag.json = "Posts.xml";
            //ViewBag.id = PostID;

            vjson = "Posts.xml";
            vid = PostID;

            return RedirectToAction("TreeGenerate");
        }
        public class postTree
        {
            public string name { get; set; }
            public List<commentTree> children { get; set; }
            public postTree(Post p)
            {
                this.name = p.Title;
                this.children = new List<commentTree>();
                foreach (var c in p.comments)
                {
                    this.children.Add(new commentTree(c.Author));
                }
            }
        }
        public class commentTree
        {
            public string name { get; set; }

            public commentTree(string title)
            {
                this.name = title;
            }
        }
    }
}