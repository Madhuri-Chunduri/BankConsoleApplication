using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface IUserService
    {
         void AddUser(User user);

         User GetUser(string userName);

         User GetLoginUser(string userName);

         User UpdateUser(User user);
    }
}
