using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollSystem.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, Display(Name = "Security Level"), EnumDataType(typeof(userRole))]
        public Enum Role { get; set; }
        [Required, StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        enum userRole
        {
            Administrator,
            User
        }
    }
}