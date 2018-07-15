using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollSystem.Models
{
    //Model for the tblPersonnel
    public class Person
    {
        public int ID { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Pay Rate")]
        [Required]
        public double PayRate { get; set; }
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }
    }
}