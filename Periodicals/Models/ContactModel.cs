using System.ComponentModel.DataAnnotations;


namespace Periodicals.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Enter your name")]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter subject of message")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Enter text of message")]
        [Display(Name = "Your text")]
        public string Text { get; set; }

        [EmailAddress]
        [Display(Name = "Your email")]
        public string Email { get; set; }
    }
}