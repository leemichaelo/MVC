function annualSalaryCalculation() {
    var weeklyWages = document.getElementById("hourlyWages").value;
    var hoursPerWeek = document.getElementById("hoursPerWeek").value;
    var numberOfWeeks = document.getElementById("numberOfWeeks").value;
    document.getElementById("annualCalculation").innerHTML = "Annual Salary: $" + (weeklyWages * hoursPerWeek * numberOfWeeks).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
}
