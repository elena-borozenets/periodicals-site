using Microsoft.AspNet.Identity.EntityFramework;

namespace Periodicals.Core.Identity
{
    public class PeriodicalsRole : IdentityRole
    {
        public PeriodicalsRole() : base() { }
        public PeriodicalsRole(string name) : base(name) { }
    }
}
