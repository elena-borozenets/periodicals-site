using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Periodicals.Core.Identity;
using Periodicals.Core.Entities;

namespace Periodicals.Infrastructure.Data
{
    public class PeriodicalDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public PeriodicalDbContext() : base()
        {
            var a = Database.Connection.ConnectionString;
        }
    }
}
