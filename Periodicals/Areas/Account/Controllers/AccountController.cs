using Microsoft.AspNet.Identity.Owin;
using Periodicals.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Periodicals.Areas.Account.Models;
using Periodicals.Core.Identity;
using Periodicals.Core.Interfaces;
using Periodicals.Exceptions;
using Periodicals.Infrastructure.Data;
using Periodicals.Infrastructure.Repositories;
using Periodicals.Models;

namespace Periodicals.Areas.Account.Controllers
{
    [PeriodicalsException]
    [IndexOutOfRangePeriodicalsException]
    [ArgumentPeriodicalsException]
    [NullReferencePeriodicalsException]
    [InvalidOperationPeriodicalsException]
    [ArgumentNullPeriodicalsException]
    [ArgumentOutOfRangePeriodicalsException]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        Logger logger = LogManager.GetCurrentClassLogger();
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: Login/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool shouldLockout = false;
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var signInStatus =
                   signInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout);
            if (signInStatus == SignInStatus.Success)
            {
                ViewBag.iii = 1;
            }
            else
            {
                ViewBag.iii = 0;
                logger.Info("user failed to login " + model.Username );
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username
            };
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = userManager.Create(newUser, model.Password);
            if (result.Succeeded)
            {
                ViewBag.o0 = "Все ок!";
                ViewBag.o1 = newUser.UserName;

                logger.Info("new user "+newUser.UserName+" is registered");

                var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                var signInStatus =signInManager.PasswordSignIn(model.Username, model.Password, false, false);
                var addUserRoleResult = userManager.AddToRole(newUser.Id, "Subscriber");
                if(addUserRoleResult.Succeeded)
                {

                }
                return View("RegisterResult");

            }
            else
            {
                ViewBag.o0 = "Все не ок!";
                ViewBag.o1 = ":(";
                return View("RegisterResult");
            }
            return View();
        }

        public ActionResult Account()
        {
            var userId = User.Identity.GetUserId();
            
                var user = _userRepository.GetById(userId);


                var userModel = new AccountViewModel() {
                    Username = user.UserName,
                    Email = user.Email,
                    Credit = user.Credit,
                    Subscriptions = EditionAccountModel.ToModelList(user.Subscription) };
                ViewBag.Blocked = user.LockoutEnabled;

            //var user = userManager.FindByName(User.Identity.Name);// var m =user.Subscription;
            

            return View(userModel);
        }


        public ActionResult LogOut()
        {
            var autentificationManager = HttpContext.GetOwinContext().Authentication;
            autentificationManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult TopUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult TopUp(TopUpModel model)
        {
            if (ModelState.IsValid&&model.Amount>0)
            {
                var userId = User.Identity.GetUserId();
                if (_userRepository.TopUp(userId, model.Amount))
                {
                    logger.Info("user"+ User.Identity.Name+" reilled the account for " + model.Amount);
                    ViewBag.TopUpResult = "Your account has been credited";
                }
            }
            else
            {
                ViewBag.TopUpResult = "Your account has not been credited";
            }

            return RedirectToAction("Account");
        }

        public ActionResult ChangeInfo()
        {
            var userId = User.Identity.GetUserId();
            var user = _userRepository.GetById(userId);
            var model = new AccountViewModel()
            {
                Username = user.UserName,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.PassMessage = "Your password has not been changed! The password and confirm password fields do not match!";
                return RedirectToAction("ChangeInfo");
            }

            if (password != null && confirmPassword != null && password == confirmPassword)
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = userManager.ChangePassword(userId, oldPassword, password);
                if (result.Succeeded)
                {
                    ViewBag.PassMessage = "Your password has been changed successfully!";
                }
                else
                {
                    ViewBag.PassMessage = "Your password has not been changed! Maybe you entered wrong password!";
                    return RedirectToAction("ChangeInfo");
                }
            }

            return RedirectToAction("ChangeInfo");
        }

        [HttpPost]
        public ActionResult ChangeUsername(AccountViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                ViewBag.UsernameMessage = "The username is not correct";
                return RedirectToAction("ChangeInfo");
            }

            var userId = User.Identity.GetUserId();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            user.UserName = model.Username;
            var result = userManager.Update(user);
            //var r = userManager.Update()
                if (result.Succeeded)
                {
                    ViewBag.UsernameMessage = "Your username has been changed successfully!";
                }
                else
                {
                ViewBag.UsernameMessage = "Your username has not been changed!";
            }

            return RedirectToAction("ChangeInfo");
        }

        [HttpPost]
        public ActionResult ChangeEmail(AccountViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ViewBag.EmailMessage = "The email is not correct";
                return RedirectToAction("ChangeInfo");
            }

            var userId = User.Identity.GetUserId();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            user.Email = model.Email;
            var result = userManager.Update(user);
            //var r = userManager.Update()
            if (result.Succeeded)
            {
                ViewBag.EmailMessage = "Your email has been changed successfully!";
            }
            else
            {
                ViewBag.EmailMessage = "Your email has not been changed!";
            }

            return RedirectToAction("ChangeInfo");
        }
    }
}