using Microsoft.AspNet.Identity.Owin;
using Periodicals.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Periodicals.Areas.Account.Models;
using Periodicals.Core.Identity;
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
        private readonly UserRepository _userRepository;
        public AccountController()
        {
            _userRepository = new UserRepository();
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
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                _userRepository.TopUp(userId, model.Amount);

                ViewBag.TopUpResult = "Your account has been credited";
            }
            else
            {
                ViewBag.TopUpResult = "Your account has not been credited";
            }

            return RedirectToAction("Account");
        }
    }
}