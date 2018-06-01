using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Periodicals.Core;
using Periodicals.Core.SharedKernel;

namespace Periodicals.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        //add your custom properties which have not included in IdentityUser before

        public float Credit { get; set; }
        //public virtual IList<int> SubscriptionEditionsId { get; set;}
        public virtual IList<Edition> Subscription { get; set; }
        public int Prop { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //if (SubscriptionEditionsId==null) SubscriptionEditionsId = new List<int>();
            // Add custom user claims here
            return userIdentity;
        }
        
    }
}
