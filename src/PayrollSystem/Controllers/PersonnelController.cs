using PayrollSystem.Data;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace PayrollSystem.Controllers
{
    public class PersonnelController : Controller
    {
        public ActionResult frmAddPersonnel()
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
        public ActionResult frmAddPersonnel(Person person)
        {
            ValidatePersonnelForm(person);

            //If the form is correct it will save it to the useractivity table and also redirect to the verify form
            if (ModelState.IsValid)
            {
                saveUserActivity("Personnel Form");
                AddPerson(person);
                return RedirectToAction("frmVerifyPersonnel", person);
            }

            return View(person);
        }

        public ActionResult frmEditPersonnel(int? id)
        {
            // If the id, throw a bad request url result
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = GetPerson((int)id);

            // If the person is null, throw a not found result
            if (person == null)
            {
                return HttpNotFound();
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult frmEditPersonnel(Person person)
        {
            ValidatePersonnelForm(person);

            if (ModelState.IsValid)
            {
                UpdatePerson(person);
                TempData["Message"] = "The personnel was succsefully updated!";

                return RedirectToAction("frmViewPersonnel");
            }

            return View("frmEditPersonnel");
        }

        public ActionResult frmDeletePersonnel(int? id)
        {
            //Verify that Id is not null, if so return bad http status
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Retrieve Person provided by id
            Person person = GetPerson((int)id);

            //Return not found if person is not found
            if(person == null)
            {
                return HttpNotFound();
            }

            //Pass Person to the view
            return View(person);
        }

        [HttpPost]
        public ActionResult frmDeletePersonnel(int id)
        {
            //TODO Delete the entry
            DeletePerson(id);

            TempData["Message"] = "The peronnel was succsefully deleted!";

            //TODO Redirect to the entries list page
            return RedirectToAction("frmViewPersonnel");
        }

        public ActionResult frmSearchPersonnel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult frmSearchPersonnel(string searchRequest)
        {
            //Builds a new user based on the parameter that the user enters in
            Person searchFor = new Person
            {
                FirstName = "",
                LastName = searchRequest,
                PayRate = 0,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

            //Sends the new person to the post viewPersonnel
            return RedirectToAction("frmViewPersonnel", searchFor);
        }

        public ActionResult frmViewPersonnel(string searchRequest)
        {
            List<Person> personnelFiles = new List<Person>();

            //Check if the user specified a last name
            if (searchRequest == "" || searchRequest == null)
            {
                personnelFiles = GetPeople();
            }
            else
            {
                personnelFiles = SearchFor(searchRequest);
            }

            return View(personnelFiles);
        }

        public ActionResult frmVerifyPersonnel(Person person)
        {
            //Displays the information that was saved during the personnel form
            ViewBag.Message = person.FirstName + "\n"
                               + person.LastName + "\n"
                               + person.PayRate + "\n"
                               + person.StartDate.ToShortDateString() + "\n"
                               + person.EndDate.ToShortDateString() + "\n"
                               + "The Information Was Successfully Saved!";

            return View();
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

        //Valiates Personnel Form
        public void ValidatePersonnelForm(Person person)
        {
            //Ensures that the end date is the same or greater than the start date
            if (ModelState.IsValidField("EndDate") && person.EndDate <= person.StartDate)
            {
                ModelState.AddModelError("EndDate", "The End Date Must Be Greater Than Or Equal To The Start Date");
            }
        }

        //Returns a list of all people
        public List<Person> GetPeople()
        {
            List<Person> allPeople = new List<Person>();

            using (var context = new Context())
            {
                allPeople = context.People
                    .OrderByDescending(l => l.LastName)
                    .ToList();
            }

            return allPeople;
        }

        //Returns a person based on given id
        public Person GetPerson(int id)
        {
            using (var context = new Context())
            {
                Person personToReturn = context.People
                    .Where(e => e.Id == id)
                    .SingleOrDefault();

                return personToReturn;
            }
        }

        //Returns matches to a search result, just last name for now
        public List<Person> SearchFor(string searchRequest)
        {
            List<Person> listToReturn = new List<Person>();

            using (var context = new Context())
            {
               listToReturn = context.People
                    .Where(m => m.LastName == searchRequest)
                    .ToList();
            }

            return listToReturn;
        }

        //Adds a Person to the Database
        public void AddPerson(Person person)
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

        //Updates a Person in the Database
        public void UpdatePerson(Person updatedInformation)
        {
            using (var context = new Context())
            {
                var personToUpdate =
                (from p in context.People
                 where p.Id == updatedInformation.Id
                 select p).First();

                context.Entry(personToUpdate).CurrentValues
                    .SetValues(updatedInformation);

                context.SaveChanges();
            }
        }

        //Removes a Person from the Database
        public void DeletePerson(int id)
        {
            using (var context = new Context())
            {
                Person personToDelete = context.People
                    .Where(e => e.Id == id)
                    .SingleOrDefault();

                context.People.Remove(personToDelete);
                context.SaveChanges();
            }
        }

    }
}