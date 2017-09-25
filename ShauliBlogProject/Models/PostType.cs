using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class PostType
    {
        [Key, ForeignKey("Post")]
        public int PostTypeID { get; set; }
        public int Sport { get; set; }
        public int Music { get; set; }
        public int Food { get; set; }
        public int Vacation { get; set; }
        public int Study { get; set; }

        public virtual Post Post { get; set; }

        public double[] getTypeArray()
        {
            double[] arr = new double[5];
            arr[0] = Sport;
            arr[1] = Music;
            arr[2] = Food;
            arr[3] = Vacation;
            arr[4] = Study;

            return arr;
        }
    }
}