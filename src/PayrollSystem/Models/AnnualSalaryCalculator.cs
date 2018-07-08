using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollSystem.Models
{
    public class AnnualSalaryCalculator
    {      
        public double HourlyWages { get; set; }
        public double HoursPerWeek { get; set; }
        public double WeeksPerYear { get; set; }
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