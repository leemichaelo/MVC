using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollSystem.ViewModels
{
    public class AccountViewModel
    {
        public class GroupedUserViewModel
        {
            public List<UserViewModel> Users { get; set; }
            public List<UserViewModel> Admins { get; set; }
        }

        public class UserViewModel
        {
            public string Username { get; set; }
            public string Role { get; set; }
        }
    }
}