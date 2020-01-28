using Bank.Models;
using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    class AccountMethods
    {
        IAccountService accountService = new AccountService();
        IUserService userService = new UserService();
        IBankService bankService = new BankService();
        CommonMethods commonMethods = new CommonMethods();

        //Create Account
        public void CreateAccount()
        {
            string username = commonMethods.ReadString("Enter account user name : ");

            User user = userService.GetUser(username);
            if (user == null)
            {
                Console.WriteLine("Sorry!! There is no user with the given username..");
                int choice = commonMethods.ValidateInt("Do you want to add a user >> 1. Yes 2. No : ");

                if (choice == 1)
                {
                    IStaffActions staffActions = new StaffActions();
                    staffActions.AddUser();
                }
                else if (choice != 1)
                {
                    int AccountDecision = commonMethods.ValidateInt("Do you want to add an account >> 1. Yes 2. No : ");
                    if (AccountDecision == 1) CreateAccount();
                    else furtherAction();
                }
            }

            List<Account> checkAccounts = accountService.GetAllAccountsByName(username);
            if(checkAccounts.Count!=0)
            {
                List<int> types = new List<int>();
                foreach (Account acccount in checkAccounts)
                {
                    types.Add(Convert.ToInt32(acccount.Type));
                }
                Console.WriteLine("Enter the type of account you want to create : ");
                for(int i=1;i<=4;i++)
                {
                    if(types.Exists(obj => obj == i)) continue;
                    else
                    {
                        Console.Write(i + " " + Enum.GetName(typeof(AccountType), i)+ " ");
                    }
                }
                Console.WriteLine("5. Exit");
                int accountDecision = commonMethods.ValidateInt("Please enter your choice : ");

                while(types.Exists(obj => obj == accountDecision) || accountDecision > 6 || accountDecision <= 0)
                {
                    if (types.Exists(obj => obj == accountDecision))
                    {
                        Console.WriteLine("Account already exists!!!");
                        int accountChoice = commonMethods.ValidateInt("Do you want to create another account : 1. Yes 2. No >> ");
                        if (accountChoice == 1) CreateAccount();
                        furtherAction();
                    }
                    if (accountDecision == 5) furtherAction();
                    accountDecision = commonMethods.ValidateInt("Please enter a valid choice : ");
                }

                Account newAccount = new Account()
                {
                    Balance=0, Id= Guid.NewGuid(),
                    Number = username.Substring(0, 3) + DateTime.Now,
                    BankId = LoginMethods.CurrentBank.Id,
                    Type = (AccountType)accountDecision
                };
                accountService.CreateAccount(newAccount);
                Console.WriteLine("Account added successfully!!!");
                furtherAction();
            }

            Console.WriteLine("Please enter the type of account you want to create : ");
            for (int i = 1; i <= 4; i++)
            {
                 Console.Write(i + " " + Enum.GetName(typeof(AccountType), i)+" ");
            }
            Console.WriteLine("5. Exit");
            int decision = commonMethods.ValidateInt("Please enter your choice : ");

            while (decision > 6 || decision <= 0)
            {
                if (decision == 5) furtherAction();
                decision = commonMethods.ValidateInt("Please enter a valid choice : ");
            }
            string accountNumber = username.Substring(0, 3) + DateTime.Now;

             Account account = new Account()
            {
                Id = Guid.NewGuid(),
                Number = accountNumber,
                Balance = 0,
                UserName = username,
                BankId = LoginMethods.CurrentBank.Id,
                Type = (AccountType)decision
            };
            accountService.CreateAccount(account);
            Console.WriteLine("Account added successfully!!!");
            furtherAction();
        }

        //Delete Account
        public void DeleteAccount()
        {
            string name = commonMethods.ReadString("Enter account user name : ");
            List<Account> accounts = accountService.GetAllAccountsByName(name);
            if(accounts==null)
            {
                Console.WriteLine("There is no account linked with the given username");
                furtherAction();
            }
            Console.WriteLine("Enter the type of account you want to delete");
            List<int> types = new List<int>();
            foreach (Account account in accounts)
            {
                types.Add(Convert.ToInt32(account.Type));
                Console.Write(Convert.ToInt32(account.Type) + " " + Enum.GetName(typeof(AccountType), account.Type) + " ");
            }
            int choice = Int32.Parse(Console.ReadLine());
            while(!types.Exists(obj=>obj==choice) || choice < 0 || choice > 6)
            {
                Console.Write("Please enter a valid choice : ");
                choice = Int32.Parse(Console.ReadLine());
            }
            Account selectedAccount = accounts.Find(obj => obj.Type == (AccountType)choice);
            accountService.DeleteAccount(selectedAccount);
            Console.WriteLine("Account deleted successfully!!!");
            furtherAction();
        }

        void furtherAction()
        {
            IUserActions userActions = new UserActions();
            userActions.furtherAction();
        }
    }
}

