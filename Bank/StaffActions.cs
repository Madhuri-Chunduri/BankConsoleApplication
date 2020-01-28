using Bank.Models;
using Bank.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Bank
{
    public class StaffActions : UserActions, IStaffActions
    {
        AccountMethods accountMethods = new AccountMethods();
        TransactionMethod transactionMethod = new TransactionMethod();
        IAccountService accountService = new AccountService();
        ValidationMethods validationMethods = new ValidationMethods();
        CommonMethods commonMethods = new CommonMethods();

        IUserService userService = new UserService();
        ICurrencyService currencyService = new CurrencyService();
        IBankService bankService = new BankService();

        public void DisplayMethods()
        {
            base.DisplayMethods();
            Console.WriteLine("7. Create new account");
            Console.WriteLine("8. Create new user");
            Console.WriteLine("9. Delete an account ");
            Console.WriteLine("10. Add new accepted currency and exchange rate");
            Console.WriteLine("11. Revert any transaction");
            TakeAction();
        }

        public void TakeAction()
        {
            int choice = commonMethods.ValidateInt("Your choice : ");
            while(choice<1 || choice > 11)
            {
                choice = commonMethods.ValidateInt("Please enter a valid choice between 1 and 11");
            }
            if (choice >= 1 && choice <= 6)
                base.TakeAction(choice);
            else
            {
                switch (choice)
                {
                    case (7):
                        accountMethods.CreateAccount();
                        break;

                    case (8):
                        AddUser();
                        break;

                    case (9):
                        accountMethods.DeleteAccount();
                        break;

                    case (10):
                        AddCurrency();
                        break;

                    case (11):
                        transactionMethod.RevertTransaction();
                        break;

                }
            }
        }

        public void AddCurrency()
        {
            Console.WriteLine("Enter new currency type and it's exchange rate");
            string currencyType = commonMethods.ReadString("Currency Type : ");
            Currency checkCurrency = currencyService.GetCurrency(currencyType.ToLower());
            double exchangeRate = 0;

            if (checkCurrency != null)
            {
                Console.WriteLine("The currency type already exists..");
                Console.WriteLine("Do you want to update exchange rate ? ");
                int choice = commonMethods.ValidateInt("1. Yes 2. No >> ");
                if (choice == 1)
                {
                    exchangeRate = commonMethods.ValidateDouble("Enter new exchange rate : ");
                    currencyService.UpdateCurrency(currencyType, exchangeRate);
                }
                furtherAction();
            }

            exchangeRate = commonMethods.ValidateDouble("Enter new exchange rate : ");
            Currency currency = new Currency()
            {
                Type = currencyType.ToLower(),
                ExchangeRate = exchangeRate,
                BankId = LoginMethods.CurrentBank.Id
            };
            currencyService.AddCurrency(currency);

            Console.WriteLine("Currency type updated successfully");
            base.furtherAction();
        }

        public void AddUser()
        {
            User user = new User();
            user.Name = commonMethods.ReadString("Enter name of the user : ");
            while (user.Name == "")
            {
                Console.WriteLine("Name Field cannot be empty");
                user.Name = commonMethods.ReadString("Please enter a valid name : ");
            }

            user.UserName = commonMethods.ReadString("Enter a userName : ");
            while (user.UserName == "")
            {
                Console.WriteLine("UserName Field cannot be empty");
                user.UserName = commonMethods.ReadString("Please enter a valid user name : ");
            }

            bool isAvailableUserName = validationMethods.IsAvailableUserName(user.UserName);
            while (isAvailableUserName == false)
            {
                Console.WriteLine("Sorry!!! This username is already taken..");
                user.UserName = commonMethods.ReadString("Please enter a new UserName : ");
                isAvailableUserName = validationMethods.IsAvailableUserName(user.UserName);
            }

            user.MobileNumber = commonMethods.ReadString("Enter Mobile Number : ");
            bool validNumber = validationMethods.IsValidPhoneNumber(user.MobileNumber);
            while (validNumber == true)
            {
                Console.WriteLine("Sorry!! This is not a valid mobile number..");
                user.MobileNumber = commonMethods.ReadString("Enter valid Mobile Number :");
                validNumber = validationMethods.IsValidPhoneNumber(user.MobileNumber);
            }

            user.Address = commonMethods.ReadString("Enter User Address : ");
            while (user.Address == "")
            {
                Console.WriteLine("Address Field cannot be empty");
                user.Address = commonMethods.ReadString("Please enter a valid address");
            }

            user.Email = commonMethods.ReadString("Enter User Email Address : ");
            bool validEmail = validationMethods.IsValidEmail(user.Email);
            while (validEmail == false)
            {
                user.Email = commonMethods.ReadString("Enter valid Email Address :");
                validEmail = validationMethods.IsValidEmail(user.Email);
            }

            int Type = commonMethods.ValidateInt("Enter Type of User : 1. staff 2. user >> ");
            if (Type == 1) user.Type = "staff";
            else user.Type = "user";

            user.Password = commonMethods.ReadString("Enter a Password to set : ");
            user.BankId = LoginMethods.CurrentBank.Id;
            userService.AddUser(user);

            Console.WriteLine("User added successfully!!!");

            if (user.Type != "staff")
            {
                string accountnumber = user.UserName.Substring(0, 3) + DateTime.Now;
                Guid accountId = Guid.NewGuid();
                Account account = new Account()
                {
                    Id = accountId,
                    Number = accountnumber,
                    Balance = 0,
                    UserName = user.UserName,
                    BankId = LoginMethods.CurrentBank.Id,
                    Type = (AccountType)1
                };
                accountService.CreateAccount(account);
                base.furtherAction();
            }
        }
    }
}
