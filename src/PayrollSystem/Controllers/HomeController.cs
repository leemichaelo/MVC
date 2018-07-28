using PayrollSystem.Data;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnnualSalaryCalculator()
        {
            //Initializes the salary with the default values
            var annualSalary = new AnnualSalaryCalculator()
            {
                HourlyWages = 0,
                HoursPerWeek = 0
            };

            //When the page loads it starts with the default values
            return View(annualSalary);
        }

        [HttpPost]
        public ActionResult AnnualSalaryCalculator(AnnualSalaryCalculator annualSalary)
        {
            //If there aren't any "HourlyWages" field validation errors
            //then make sure the HourlyWages is greater than 0
            if (ModelState.IsValidField("HourlyWages") && annualSalary.HourlyWages <= 0)
            {
                ModelState.AddModelError("HourlyWages", "The Hourly Wages field must be greater than '0'");
            }

            //If there aren't any "HoursPerWeek" field validation errors
            //then make sure the HourlyWages is greater than 0
            if (ModelState.IsValidField("HoursPerWeek") && annualSalary.HoursPerWeek <= 0)
            {
                ModelState.AddModelError("HoursPerWeek", "The Weekly Hours field must be greater than '0'");
            }

            if (ModelState.IsValidField("WeeksPerYear") && annualSalary.WeeksPerYear <= 0)
            {
                ModelState.AddModelError("WeeksPerYear", "The Weeks Per Year field must be greater than '0'");
            }

            //Validates the values against the object
            if (ModelState.IsValid)
            {
                //When the user hits the calculate button in the view, this calculates the annual salary
                double annualSalaryCalculation = (annualSalary.HourlyWages * annualSalary.HoursPerWeek) * annualSalary.WeeksPerYear;
                ViewBag.AnnualSalary = String.Format("{0:C}", annualSalaryCalculation);

                return View();
            }

            //If the model state is not valid it will keep the values on the page
            return View(annualSalary);
        }

        public ActionResult frmPersonnel()
        {
            //Initializes the personnel form to have the date fields default to today
            var person = new Person()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };
            //When the page loads it will start with the person object
            return View(person);
        }

        [HttpPost]
        public ActionResult frmPersonnel(Person person)
        {
            if (ModelState.IsValidField("EndDate") && person.EndDate <= person.StartDate)
            {
                ModelState.AddModelError("EndDate", "The End Date Must Be Greater Than Or Equal To The Start Date");
            }

            if (ModelState.IsValid)
            {
                saveUserActivity("Personnel Form");
                return RedirectToAction("frmPersonnelVerify", person);
            }

            return View(person);
        }

        public ActionResult frmPersonnelVerify(Person person)
        {
            ViewBag.Message = person.FirstName + "\n"
                               + person.LastName + "\n"
                               + person.PayRate + "\n"
                               + person.StartDate.ToShortDateString() + "\n"
                               + person.EndDate.ToShortDateString();

            return View();
        }

        public ActionResult frmUserActivity()
        {
            List <UserActivity> userActivities= loadUserActivity();

            return View(userActivities);
        }

        public void saveUserActivity(string frmAccessed)
        {
            using (var context = new Context())
            {
                context.UserActivites.Add(new Models.UserActivity()
                {
                    UserIP = Request.UserHostAddress,
                    DateOfActivity = DateTime.Now,
                    FormAccessed = frmAccessed
                });

                context.SaveChanges();
            }
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