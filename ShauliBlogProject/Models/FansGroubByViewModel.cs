using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class FansGroubByViewModel
    {
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Count is required.")]
        public int Count { get; set; }
        public double percent { get; set; }


        public FansGroubByViewModel(string country, int count)
        {
            this.Country = country;
            this.Count = count;
        }
    }
}