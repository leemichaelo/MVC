using Foolproof;
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
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Pay Rate")]
        public double PayRate { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}