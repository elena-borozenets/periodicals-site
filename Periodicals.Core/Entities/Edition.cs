using Periodicals.Core.SharedKernel;
using System;
using System.Collections.Generic;
using Periodicals.Core.Identity;

namespace Periodicals.Core.Entities
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
        public string Image { get; set; }

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
