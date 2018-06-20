using Periodicals.Core.Identity;

namespace Periodicals.Core.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser GetById(string userId);
        bool TopUp(string userId, float amount);
    }
}
