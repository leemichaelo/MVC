using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollSystem.ViewModels
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, Display(Name = "Security Level")]
        public string UserRole { get; set; }
    }
}