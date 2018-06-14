using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Periodicals.Areas.Account.Models
{
    public class TopUpModel
    {
        public float Amount { get; set; }
        public string Card { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public int CVV { get; set; }
    }
}