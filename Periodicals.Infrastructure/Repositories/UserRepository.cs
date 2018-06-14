using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core.Identity;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class UserRepository
    {

        public ApplicationUser GetById(string userId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var result = (from user in db.Users where user.Id == userId select user)
                    .Include(e => e.Subscription).FirstOrDefault();

                return result;
            }
        }

        public bool TopUp(string userId, float amount)
        {
            using (var db = new PeriodicalDbContext())
            {
                var user = (from user1 in db.Users where user1.Id == userId select user1).FirstOrDefault();
                if(user!=null) user.Credit += amount;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
