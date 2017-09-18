using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string url_author_comment { get; set; }
        public string content_comment { get; set; }

        public virtual Post post { get; set; }
    }
}