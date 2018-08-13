using Microsoft.AspNet.Identity;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollSystem.Secuirty
{
    public class ApplicationUserManager : UserManager<UserLogin>
    {
        public ApplicationUserManager(IUserStore<UserLogin> userStore) : base(userStore)
        {

        }
    }
}