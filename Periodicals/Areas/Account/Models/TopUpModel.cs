using System.ComponentModel.DataAnnotations;

namespace Periodicals.Areas.Account.Models
{
    public class TopUpModel
    {
        [Required(ErrorMessage = "Enter the amount of the replenishment")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "Enter card number")]
        [RegularExpression(@"[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}", 
            ErrorMessage = "The format should be ####-####-####-####, where # - 0-9 numbers")]
        public string Card { get; set; }

        [Required(ErrorMessage = "Enter mounth")]
        [Range(01, 12, ErrorMessage = "Inadmissible mounth")]
        public int DateMonth { get; set; }

        [Required(ErrorMessage = "Enter year")]
        [Range(0, 99, ErrorMessage = "Inadmissible year")]
        public int DateYear { get; set; }

        [Required(ErrorMessage = "Enter your CVV")]
        [RegularExpression(@"[0-9]{3}", ErrorMessage = "It should be three numbers!")]
        public int CVV { get; set; }
    }
}