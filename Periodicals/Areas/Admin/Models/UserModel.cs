using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Routing.Constraints;
using Periodicals.Core.Identity;

namespace Periodicals.Areas.Admin.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public  float Credit { get; set; }
        public bool isBlocked { get; set; }
        public bool isModerator { get; set; }

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
                    isBlocked = user.LockoutEnabled,
                    isModerator = user.Roles.Any(m=>m.RoleId == "333")
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