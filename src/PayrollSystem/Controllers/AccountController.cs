using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PayrollSystem.Data;
using PayrollSystem.Models;
using PayrollSystem.Secuirty;
using PayrollSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = applicationUserManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpGet]
        public ActionResult frmManageUsers()
        {
            AccountViewModel.GroupedUserViewModel userFiles = new AccountViewModel.GroupedUserViewModel();
            userFiles = GetUsers();

            return View(userFiles);

        }

        [HttpPost]
        public async Task<ActionResult> frmManageUsers(AccountRegisterViewModel viewModel)
        {
            //If the ModelState is valid...
            if (ModelState.IsValid)
            {
                //Instantiate a UserLogin object
                var user = new UserLogin { UserName = viewModel.Username };
                //Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                //If the user was successfuly created...
                if (result.Succeeded)
                {
                    //Sign-in the user and redirect them to the web app's "Home" page
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                //If there were errors...
                //Add model errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(viewModel);
        }

        public ActionResult frmEditUser(string username)
        {
            // If the id, throw a bad request url result
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserLogin person = GetUser(username);

            // If the person is null, throw a not found result
            if (person == null)
            {
                return HttpNotFound();
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult frmEditUser(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                UpdateUser(userLogin);
                TempData["Message"] = "The user was succsefully updated!";

                return RedirectToAction("frmViewUsers");
            }

            return View("frmEditUser");
        }

        public ActionResult frmDeleteUser(string username)
        {
            //Verify that Id is not null, if so return bad http status
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Retrieve Person provided by id
            UserLogin userLogin = GetUser(username);

            //Return not found if person is not found
            if (userLogin == null)
            {
                return HttpNotFound();
            }

            //Pass Person to the view
            return View(userLogin);
        }

        [HttpPost]
        public ActionResult frmDeleteUser(UserLogin userLogin)
        {
            //TODO Delete the entry
            DeleteUser(userLogin.UserName);

            TempData["Message"] = "The peronnel was succsefully deleted!";

            //TODO Redirect to the entries list page
            return RedirectToAction("frmViewUsers");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            // If the ModelState is valid...
            if (ModelState.IsValid)
            {
                // Instantiate a User object
                var user = new UserLogin { UserName = viewModel.Username };

                // Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                // If the user was successfully created...
                if (result.Succeeded)
                {
                    // Sign-in the user
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Redirect them to the web app's "Home" page        
                    return RedirectToAction("Index", "Entries");
                }

                // If there were errors...
                // Add model errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignIn(AccountSignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            //Sign-In the user
            var result = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, viewModel.RememberMe, shouldLockout: false);

            //Check the Result
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(viewModel);
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException("identity feature not implemented.");
                default:
                    throw new Exception("Unexpected Microsoft.AspNet.Identity.Owin.SIgnInStatus enum value: " + result);
            }

        }

        [HttpPost]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("SignIn", "Account");
        }

        //Returns a list of all users
        public AccountViewModel.GroupedUserViewModel GetUsers()
        {
            using (var context = new Context())
            {
                //allUsers = context.Users
                //    .OrderByDescending(u => u.UserName)
                //    .ToList();

                var role = (from r in context.Roles where r.Name.Contains("User") select r).FirstOrDefault();
                var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

                var userVM = users.Select(user => new AccountViewModel.UserViewModel
                {
                    Username = user.UserName,
                    Role = "User"
                }).ToList();


                var role2 = (from r in context.Roles where r.Name.Contains("Administrator") select r).FirstOrDefault();
                var admins = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role2.Id)).ToList();

                var adminVM = admins.Select(user => new AccountViewModel.UserViewModel
                {
                    Username = user.UserName,
                    Role = "Administrator"
                }).ToList();


                var model = new AccountViewModel.GroupedUserViewModel { Users = userVM, Admins = adminVM };

                return model;
            }
        }

        //Returns a user based on given id
        public UserLogin GetUser(string username)
        {
            using (var context = new Context())
            {
                UserLogin userToReturn = context.Users
                    .Where(u => u.UserName == username)
                    .SingleOrDefault();

                return userToReturn;
            }
        }

        //Updates a User in the Database
        public void UpdateUser(UserLogin updatedInformation)
        {
            using (var context = new Context())
            {
                var userToUpdate = context.Users
                    .Where(u => u.UserName == updatedInformation.UserName)
                    .SingleOrDefault();

                context.Entry(userToUpdate).CurrentValues
                    .SetValues(updatedInformation);

                context.SaveChanges();
            }
        }

        //Removes a User from the Database
        public void DeleteUser(string username)
        {
            using (var context = new Context())
            {
                UserLogin userToDelete = context.Users
                    .Where(u => u.UserName == username)
                    .SingleOrDefault();


                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
        }
    }

}