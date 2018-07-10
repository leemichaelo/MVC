using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace PayrollSystem.Models
{
    public class AnnualSalaryCalculator
    {
        [Display(Name = "Hourly Wages")]
        [DataType(DataType.Currency, ErrorMessage = "The Hourly Wages Field Must Be Formated as '0.00'")]
        public double HourlyWages { get; set; }
        [Display(Name = "Weekly Hours")]
        public double HoursPerWeek { get; set; }
        [Display(Name = "Weeks Per Year")]
        public double WeeksPerYear { get; set; }
        [Display(Name = "Annual Salary")]
        public double AnnualSalary { get; set; }

        public AnnualSalaryCalculator()
        {

        }

        public AnnualSalaryCalculator(double hourlyWages, double hoursPerWeek, double weeksPerYear)
        {
            HourlyWages = hourlyWages;
            HoursPerWeek = hoursPerWeek;
            WeeksPerYear = weeksPerYear;
            AnnualSalary = 0;
        }
    }
}