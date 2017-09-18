using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlogProject.Models
{
    public class FansClub
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use letters only please")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use letters only please")]
        public String LastName { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public String Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Birthday is required.")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Seniority is required.")]
        public int Seniority { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Adress is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}