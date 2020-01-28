using Bank.Models;
using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class LoginMethods
    {
        public static User CurrentUser = new User();
        public static Bank CurrentBank = new Bank();
        public static Account CurrentAccount = new Account();
        CommonMethods commonMethods = new CommonMethods();

        IBankService bankService = new BankService();
        IUserService userService = new UserService();
        IAccountService accountService = new AccountService();

        public void ValidateUser()
        {
            Console.Clear();

            Console.WriteLine("<----------WELCOME TO LOGIN PAGE---------->");

            string userName = commonMethods.ReadString("Enter your username : ");

            CurrentUser = userService.GetLoginUser(userName);

            while (CurrentUser == null)
            {
                Console.WriteLine("Incorrect UserName, Please enter a valid username");
                userName = commonMethods.ReadString("Enter a valid username : ");
                CurrentUser = userService.GetLoginUser(userName);
            }

            string password = commonMethods.ReadString("Enter your password : ");
            string actualPassword = CurrentUser.Password;
            if (password == actualPassword)
            {
                if (userName == "admin")
                {
                    Console.Clear();
                    AdminMethods adminMethods = new AdminMethods();
                    adminMethods.AdminActions();
                }
                GetBank(userName);
                List<Account> accounts = accountService.GetAllAccountsByName(userName);
                if(accounts.Count==0 && CurrentUser.Type == "staff")
                {
                    CurrentAccount = null;
                    StaffActions staff = new StaffActions();
                    staff.DisplayMethods();
                }
                else if(accounts.Count==1)
                {
                    CurrentAccount = accounts[0];
                    if(CurrentUser.Type=="staff")
                    {
                        StaffActions staff = new StaffActions();
                        staff.DisplayMethods();
                    }
                    UserActions user = new UserActions();
                    user.SelectAction();
                }
                else
                {
                    Console.Write("Enter the account you want to login into : ");
                    int i = 1;
                    foreach(Account account in accounts)
                    {
                        Console.Write(i + " " + Enum.GetName(typeof(AccountType), account.Type) + " ");
                        i += 1;
                    }
                    Console.WriteLine();
                    int choice = commonMethods.ValidateInt("Please enter your account choice : ");
                    while((!accounts.Exists(obj=>Convert.ToInt32(obj.Type)==choice)) || choice > accounts.Count || choice<0 )
                    {
                        Console.Write("Please enter a valid choice : ");
                        choice = Int32.Parse(Console.ReadLine());
                    }
                    CurrentAccount = accounts[choice-1];
                    if(CurrentUser.Type=="staff")
                    {
                        StaffActions staff = new StaffActions();
                        staff.DisplayMethods();
                    }
                    UserActions user = new UserActions();
                    user.SelectAction();
                }
            }
            else
            {
                Console.WriteLine("Sorry!! You've entered invalid credentials..");
                Console.WriteLine("Please enter valid credentials ");
                System.Threading.Thread.Sleep(1500);
                ValidateUser();
            }
        }

        void GetBank(string userName)
        { 
            if (userName!="admin")
            {
                string bankName = commonMethods.ReadString("Please enter your bank name : ");
                CurrentBank = bankService.GetBankByName(bankName.ToLower());
                if (CurrentBank == null)
                {
                  Console.WriteLine("No bank exists with the given name");
                  while(CurrentBank==null)
                  {
                      bankName = commonMethods.ReadString("Please enter a valid bank name : ");
                      CurrentBank = bankService.GetBankByName(bankName.ToLower());
                  }
                }

            }
        }
    }
}
