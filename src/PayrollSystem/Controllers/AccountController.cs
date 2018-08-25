using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PayrollSystem.Data;
using PayrollSystem.Models;
using PayrollSystem.Secuirty;
using PayrollSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PayrollSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : Controller
    {
        Context context;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager, IAuthenticationManager authenticationManager)
        {
            context = new Context();

            _userManager = applicationUserManager;
            _signInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpGet]
        public ActionResult frmManageUsers()
        {
            List<UserLogin> userFiles = new List<UserLogin>();
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

        [HttpGet]
        public ActionResult frmEditUser(string username)
        {
            // If the id, throw a bad request url result
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserLogin editUser = GetUser(username);

            // If the person is null, throw a not found result
            if (editUser == null)
            {
                return HttpNotFound();
            }

            return View(editUser);
        }

        [HttpPost]
        public ActionResult frmEditUser(AccountViewModel user)
        {
            if (ModelState.IsValid)
            {
                AccountViewModel editUser = new AccountViewModel { Id = user.Id, UserName = user.UserName, UserRole = user.UserRole };
                UpdateUser(user);
                TempData["Message"] = "The user was succsefully updated!";

                return RedirectToAction("frmManageUsers");
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
            return RedirectToAction("frmManageUsers");
        }

        [HttpGet]
        public ActionResult frmAddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> frmAddUser(AccountRegisterViewModel viewModel)
        {
            // If the ModelState is valid...
            if (ModelState.IsValid)
            {
                // Instantiate a User object
                var user = new UserLogin { UserName = viewModel.Username, UserRole = viewModel.UserRole };

                // Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                // If the user was successfully created...
                if (result.Succeeded)
                {
                    //// Sign-in the user
                    //await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    //Assign User to Role
                    await _userManager.AddToRoleAsync(user.Id, viewModel.UserRole);

                    // Redirect them to the web app's "Home" page        
                    return RedirectToAction("Index", "Home");
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
        public ActionResult frmUserSignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> frmUserSignIn(AccountSignInViewModel viewModel)
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

                    try
                    {
                        MailMessage mail = new MailMessage();
                        //Change who you want to send the email to
                        mail.To.Add("Email sent to");
                        //Change who you want to send the email from
                        mail.From = new MailAddress("Email sent from");
                        mail.Subject = "Requesting Information";

                        string userMessage = "";
                        userMessage = "<br/>Name: " + viewModel.Username;
                        string Body = "Hi, <br/><br/> Unknown User Login:<br/><br/> " + userMessage + "<br/><br/>Thanks";

                        mail.Body = Body;
                        mail.IsBodyHtml = true;

                        SmtpClient smtp = new SmtpClient();
                        //SMTP Server Address of gmail
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        //Add in email credentials 
                        smtp.Credentials = new NetworkCredential("email username", "email password");
                        // Smtp Email ID and Password For authentication
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        ViewBag.Message = "Thank you for your time.";
                    }
                    catch
                    {
                        ViewBag.Message = "Error............";
                    }

                    return View(viewModel);
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException("identity feature not implemented.");
                default:
                    throw new Exception("Unexpected Microsoft.AspNet.Identity.Owin.SignInStatus enum value: " + result);
            }
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("frmUserSignIn", "Account");
        }

        //Returns a list of all users
        public List<UserLogin> GetUsers()
        {
            List<UserLogin> listToReturn = new List<UserLogin>();
            using (var context = new Context())
            {
                listToReturn = context.Users
                    .OrderByDescending(u => u.UserName)
                    .ToList();
            }
            return listToReturn;
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
        public void UpdateUser(AccountViewModel userName)
        {
            using (var context = new Context())
            {
                UserLogin userToUpdate = context.Users
                    .Where(u => u.Id == userName.Id)
                    .SingleOrDefault();

                _userManager.AddToRoleAsync(userToUpdate.Id, userName.UserRole);
                userToUpdate.UserRole = userName.UserRole;

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