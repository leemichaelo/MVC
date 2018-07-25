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
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Pay Rate")]
        public Nullable<double> PayRate { get; set; }
        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [Display(Name = "End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}