using Periodicals.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core.Identity;
using Periodicals.Core.Entities;

namespace Periodicals.Core
{
    public class Edition : BaseEntity, IEquatable<Edition>
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string ISSN { get; set; }
        public DateTime DateNextPublication { get; set; }
        public string Type{ get; set; }
        public Topic Topic { get; set; }
        public int Periodicity { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }

        public virtual IList<ApplicationUser> Subscribers { get; set; }

        public virtual IList<Review> Reviews { get; set; }

        public bool Equals(Edition other)
        {
            if (this.Id == other.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
