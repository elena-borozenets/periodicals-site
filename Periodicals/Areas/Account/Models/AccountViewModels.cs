using Periodicals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Periodicals.Areas.Account.Models;

namespace Periodicals.Areas.Account.Models
{
    public class LoginViewModel
    {

        /*[Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }*/
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Your email")]
        public string Email { get; set; }

        [Required]
        //[DataType(Dat)]
        [Display(Name = "Your username")]
        public string Username { get; set; }

        [Display(Name = "Credit")]
        public float Credit { get; set; }

        [Display(Name = "Your subscriptions:")]
        public List<EditionAccountModel> Subscriptions { get; set; }


    }
}