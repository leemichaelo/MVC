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
            var annualSalary = new AnnualSalaryCalculator()
            {
                HourlyWages = 00.00,
                HoursPerWeek = 0
            };

            return View(annualSalary);
        }

        [HttpPost]
        public ActionResult AnnualSalaryCalculator(AnnualSalaryCalculator annualSalary)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AnnualSalary = (annualSalary.HourlyWages * annualSalary.HoursPerWeek) * annualSalary.WeeksPerYear;
                return View();
            }

            return View(annualSalary);
        }

    }
}