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
                ViewBag.AnnualSalary = (annualSalary.HourlyWages * annualSalary.HoursPerWeek) * annualSalary.WeeksPerYear;
                return View();
            }

            //If the model state is not valid it will keep the values on the page
            return View(annualSalary);
        }

    }
}