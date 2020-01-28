using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Services
{
    class UserService : IUserService
    {
        public static List<User> Users = new List<User>();

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public User GetLoginUser(string userName)
        {
            return Users.Find(obj => (obj.UserName == userName));
        }

        public User GetUser(string userName)
        {
            return Users.Find(obj => (obj.UserName == userName && obj.BankId==LoginMethods.CurrentBank.Id));
        }

        public User UpdateUser(User user)
        {
            User updatedUser = Users.Where(obj => (obj.UserName == LoginMethods.CurrentUser.UserName && obj.BankId == LoginMethods.CurrentBank.Id))
            .Select(obj => { obj = user; return obj; }).ToList()[0];
            return updatedUser;
        }
    }
}
