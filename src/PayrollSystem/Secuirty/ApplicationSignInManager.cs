using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollSystem.Secuirty
{
    public class ApplicationSignInManager : SignInManager<UserLogin, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}