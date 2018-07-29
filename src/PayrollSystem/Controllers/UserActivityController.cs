using PayrollSystem.Data;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    public class UserActivityController : Controller
    {
        public ActionResult frmUserActivity()
        {
            List<UserActivity> userActivities = loadUserActivity();

            return View(userActivities);
        }

        public List<UserActivity> loadUserActivity()
        {
            using (var context = new Context())
            {
                var userActivity = context.UserActivites.ToList();

                return userActivity;
            }
        }
    }
}