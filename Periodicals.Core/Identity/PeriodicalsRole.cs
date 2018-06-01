using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.Core.Identity
{
    public class PeriodicalsRole : IdentityRole
    {
        public PeriodicalsRole() : base() { }
        public PeriodicalsRole(string name) : base(name) { }
    }
}
