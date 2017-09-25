using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class UserType
    {
        [Key]
        public int UserTypeID { get; set; }
        public string UserID { get; set; }
        public int PostID { get; set; }
        public int Visit { get; set; }
    }
}