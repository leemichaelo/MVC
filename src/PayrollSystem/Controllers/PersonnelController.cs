using PayrollSystem.Data;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    public class PersonnelController : Controller
    {
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
                savePersonnel(person);
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
                               + person.EndDate.ToShortDateString() + "\n"
                               + "The Information Was Successfully Saved!";

            return View();
        }

        public ActionResult frmSearchPersonnel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmSearchPersonnel(string searchRequest)
        {
            Person searchFor = new Person
            {
                FirstName = "",
                LastName = searchRequest,
                PayRate = 0,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

            return RedirectToAction("frmViewPersonnel", searchFor);
        }

        public ActionResult frmViewPersonnel(Person searchFor)
        {
            List<Person> personnelFiles = matchingPersonnelFiles(searchFor);

            return View(personnelFiles);
        }

        public void savePersonnel(Person person)
        {
            using (var context = new Context())
            {
                context.People.Add(new Models.Person()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    PayRate = person.PayRate,
                    StartDate = person.StartDate,
                    EndDate = person.EndDate
                });
                context.SaveChanges();
            }
        }

        public List<Person> matchingPersonnelFiles(Person searchRequest)
        {
            using (var context = new Context())
            {
                List<Person> personnelFiles;

                if (searchRequest.LastName == null)
                {
                    personnelFiles = context.People.ToList();
                }
                else
                {
                    personnelFiles = context.People.Where(m => m.LastName == searchRequest.LastName).ToList();
                }

                return personnelFiles;
            }
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
    }
}