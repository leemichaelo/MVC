using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollSystem.Models
{
    public class UserLogin : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        //Properties are inherited by the Identity User class
        [Display(Name="User Name")]
        public override string UserName { get; set; }
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
    }
}