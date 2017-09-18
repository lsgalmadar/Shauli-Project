using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class Fan
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime birthDate { get; set; }
        [Required]
        [Display(Name = "Seniority")]
        public int seniority { get; set; }
    }
}