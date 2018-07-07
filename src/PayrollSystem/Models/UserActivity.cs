using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollSystem.Models
{
    public class UserActivity
    {
        public int ActivityID { get; set; }
        public string UserIP { get; set; }
        public DateTime DateOfActivity { get; set; }
        public string FormAccessed { get; set; }
    }
}