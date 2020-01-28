using Bank.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Bank
{
    class ValidationMethods
    {
        IUserService userService = new UserService();

        public bool IsValidEmail(string mail)
        {
            try
            {
                MailAddress m = new MailAddress(mail);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsValidPhoneNumber(string number)
        {
            if (number.Length < 10 || number.Length > 12) return false;
            return Regex.Match(number, @"^[0-9]\d*$").Success;
        }

        public bool IsAvailableUserName(string UserName)
        {
            User user = userService.GetUser(UserName);
            if (user == null) return true;
            else return false;
        }
    }
}
