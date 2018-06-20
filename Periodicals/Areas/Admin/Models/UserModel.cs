using System.Collections.Generic;
using System.Linq;
using Periodicals.Core.Identity;

namespace Periodicals.Areas.Admin.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public  float Credit { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsModerator { get; set; }

        public static UserModel FromUser(ApplicationUser user)
        {
            if (user != null)
            {
                var userModel = new UserModel()
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    Credit=user.Credit,
                    IsBlocked = user.LockoutEnabled,
                    IsModerator = user.Roles.Any(m=>m.RoleId == "333")
                };
                return userModel;
            }
            return null;
        }



        public static List<UserModel> FromUserList(IList<ApplicationUser> users)
        {
            var userModelList = new List<UserModel>();
            foreach (var user in users)
            {
                userModelList.Add(UserModel.FromUser(user));
            }

            return userModelList;
        }
    }
}