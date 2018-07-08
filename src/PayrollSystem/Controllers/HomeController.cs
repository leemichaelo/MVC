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
                HourlyWages = 00.00,
                HoursPerWeek = 0
            };

            //When the page loads it starts with the default values
            return View(annualSalary);
        }

        [HttpPost]
        public ActionResult AnnualSalaryCalculator(AnnualSalaryCalculator annualSalary)
        {
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