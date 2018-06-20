using Microsoft.AspNet.Identity.Owin;
using Periodicals.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
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
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        Logger logger = LogManager.GetCurrentClassLogger();
        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;
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
                ViewBag.signInStatus = 1;
                logger.Info("user login to site " + model.Username);
            }
            else
            {
                ViewBag.signInStatus = false;
                ViewBag.signInMessage = "Incorrectly entered login or password";
                logger.Info("user failed to login " + model.Username );
                return View(model);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("RegisterNew");
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("RegisterNew", model);
            }

            var newUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username
            };
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = userManager.Create(newUser, model.Password);
            if (result.Succeeded)
            {ViewBag.o1 = newUser.UserName;
                ViewBag.o0 = "It's Ok! Return to main page to log in!";

                logger.Info("new user "+newUser.UserName+" is registered");

                //var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                //var signInStatus =signInManager.PasswordSignIn(model.Username, model.Password, false, false);
                var addUserRoleResult = userManager.AddToRole(newUser.Id, "Subscriber");
                if(addUserRoleResult.Succeeded)
                {
                    return View("RegisterResult");
                    //return RedirectToAction("Account");
                }
                return View("RegisterResult");

            }
            else
            {
                var errors = new List<string>(){ "It's not ok! User with such email or username is registered"};
                return View("Error", errors);
                //ViewBag.o0 = "It's not ok! Maybe you";
                //ViewBag.o1 = ":(";
                
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
                    logger.Info("user changed password " + User.Identity.Name);
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
                    logger.Info("user changed username " + userId);
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
                logger.Info("user changed email " + userId);
            }
            else
            {
                ViewBag.EmailMessage = "Your email has not been changed!";
            }

            return RedirectToAction("ChangeInfo");
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string userString)
        {

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var resultEmail = userManager.FindByEmail(userString);
            var resultName = userManager.FindByName(userString);
            if (resultName == null && resultEmail == null)
            {
                ViewBag.ForgotPasswordResult = "User with such email or username is not found";
                return View();
            }
            else
            {
                ApplicationUser user;
                if(resultName?.Email!=null)
                {
                    user = resultName;
                //userManager.SendEmail(resultName.Id, "Confirm changing password", "");
                }
                else if (resultEmail != null)
                {
                    user = resultEmail;
                    //userManager.SendEmail();
                }
                else
                {
                    return View();

                }

                var provider = new DpapiDataProtectionProvider("Sample");
                userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("EmailConfirmation"));

                string code = userManager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                MailAddress from = new MailAddress("elena.borozenets.applications@gmail.com");
                MailAddress to = new MailAddress(user.Email);
                MailMessage message = new MailMessage(from, to);

                message.Subject = "Сброс пароля";
                message.Body = $" {"Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrl + "\">сбросить</a>"}";
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("elena.borozenets.applications@gmail.com", "Some00Pass11");
                smtp.EnableSsl = true;

                smtp.Send(message);
                //userManager.SendEmail(user.Id, "Сброс пароля",
                //    "Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrl + "\">сбросить</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
              }
            return View();
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string userString, string password, string confirmPassword)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var resultEmail = userManager.FindByEmail(userString);
            var resultName = userManager.FindByName(userString);
            if (resultName == null && resultEmail == null)
            {
                ViewBag.ResetPasswordResult = "User with such email or username is not found";
                return View();
            }
            ApplicationUser user;
            if (resultName?.Email != null)
            {
                user = resultName;
                //userManager.SendEmail(resultName.Id, "Confirm changing password", "");
            }
            else if (resultEmail != null)
            {
                user = resultEmail;
                //userManager.SendEmail();
            }
            else
            {
                    return View();

            }
            if (password != confirmPassword)
            {
                ViewBag.ResetPasswordResult ="Your password has not been changed! The password and confirm password fields do not match!";
                return View();

            }
            var provider = new DpapiDataProtectionProvider("Sample");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                provider.Create("ResetingPassword"));

            string token = userManager.GeneratePasswordResetToken(user.Id);
            var result = userManager.ResetPassword(user.Id, token, password);
            if (result.Succeeded) {
                return View("ResetPasswordConfirmation");
                //ViewBag.ResetPasswordResult = "Your password has been changed successfully!";

            }
                return View();
        }

        /*public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }*/

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            if(returnUrl==null) returnUrl= "http://localhost:58373";
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback",
                    new { returnUrl = returnUrl })
            };

            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult FbLogin(string returnUrl)
        {
            if (returnUrl == null) returnUrl = "http://localhost:58373";
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback",
                    new { returnUrl = returnUrl })
            };

            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Facebook");
            return new HttpUnauthorizedResult();
        }
        /*
        [AllowAnonymous]
        public ActionResult GoogleLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = AuthManager.GetExternalLoginInfo();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Find(loginInfo.Login);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                    Credit = 0
                };

                IdentityResult result = userManager.Create(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = userManager.AddLogin(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = userManager.CreateIdentity(user,
                DefaultAuthenticationTypes.ApplicationCookie);

            ident.AddClaims(loginInfo.ExternalIdentity.Claims);

            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);

            return Redirect(returnUrl ?? "/");
        }*/

        [AllowAnonymous]
      public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
         {
             ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
             var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindAsync(loginInfo.Login);

             if (user == null)
             {
                 user = new ApplicationUser
                 {
                     Email = loginInfo.Email,
                     UserName = loginInfo.DefaultUserName,
                     Credit = 0
                 };

                 IdentityResult result = await userManager.CreateAsync(user);
                 if (!result.Succeeded)
                 {
                     return View("Error", result.Errors);
                 }
                 else
                 {
                     var newUser= userManager.FindByEmail(loginInfo.Email);
                     var addUserRoleResult = userManager.AddToRole(newUser.Id, "Subscriber");
                    result = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                     if (!result.Succeeded)
                     {
                         return View("Error", result.Errors);
                     }
                 }
             }

             ClaimsIdentity ident = await userManager.CreateIdentityAsync(user,
                 DefaultAuthenticationTypes.ApplicationCookie);

             ident.AddClaims(loginInfo.ExternalIdentity.Claims);

             AuthManager.SignIn(new AuthenticationProperties
             {
                 IsPersistent = false
             }, ident);

             return Redirect(returnUrl ?? "/");
         }

    }
}