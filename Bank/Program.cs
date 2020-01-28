using Bank.Services;
using System;
using System.Collections.Generic;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            IUserService userService = new UserService();
            User user = new User()
            {
                Name = "Admin",
                UserName = "admin",
                MobileNumber = "9999999999",
                Password = "admin",
                Type = "staff",
                Address = "Hyderabad",
                Email = "admin@technovert.net"
            };
            userService.AddUser(user);
            LoginMethods login = new LoginMethods();
            login.ValidateUser();
        }
    }
}
