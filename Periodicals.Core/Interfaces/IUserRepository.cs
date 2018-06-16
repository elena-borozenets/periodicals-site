using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core.Identity;

namespace Periodicals.Core.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser GetById(string userId);
        bool TopUp(string userId, float amount);
    }
}
