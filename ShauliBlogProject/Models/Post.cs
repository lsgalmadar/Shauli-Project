using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string url_author_post { get; set; }
        public DateTime postDate { get; set; }
        public string contentPost { get; set; }
        public string image { get; set; }
        public string video { get; set; }

        public virtual ICollection<Comment> comments { get; set; }
    }
}