using Bank.Models;
using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    class TransactionMethod
    {
        LoginMethods login = new LoginMethods();
        ITransactionService transactionService = new TransactionService();
        IAccountService accountService = new AccountService();
        IBankService bankService = new BankService();
        ICurrencyService currencyService = new CurrencyService();
        AccountMethods accountMethods = new AccountMethods();
        CommonMethods commonMethods = new CommonMethods();

        public void CheckAccount()
        {
            List<Account> accounts = accountService.GetAllAccountsByName(LoginMethods.CurrentUser.UserName);
            if (accounts.Count==0)
            {
                Console.WriteLine("Sorry!! You don't have an account with " + LoginMethods.CurrentBank.Name + " to complete the transaction..");
                if (LoginMethods.CurrentUser.Type == "staff")
                {
                    int choice = commonMethods.ValidateInt("Want to create an account : 1 Yes 2. No >> ");
                    if (choice == 1) accountMethods.CreateAccount();
                    else furtherAction();
                }
            }
            if (accounts.Count == 1) LoginMethods.CurrentAccount = accounts[0];
            else
            {
                Console.Write("Enter the account you want to perform actions on : ");
                int i = 1;
                foreach (Account account in accounts)
                {
                    Console.Write(i + " " + Enum.GetName(typeof(AccountType), account.Type) + " ");
                    i += 1;
                }
                Console.WriteLine();
                int choice = commonMethods.ValidateInt("Please enter your account choice : ");
                while ((!accounts.Exists(obj => Convert.ToInt32(obj.Type) == choice)) || choice > accounts.Count || choice < 0)
                {
                    choice = commonMethods.ValidateInt("Please enter a valid choice : ");
                }
                LoginMethods.CurrentAccount = accounts[choice - 1];
            }
        }

        public void WithDraw()
        {
            if(LoginMethods.CurrentAccount==null) CheckAccount();
            double amount =commonMethods.ValidateDouble("Enter the amount you want to withdraw : ");
            LoginMethods.CurrentAccount.Balance = LoginMethods.CurrentAccount.Balance - amount;

            if (amount > LoginMethods.CurrentAccount.Balance)
            {
                Console.WriteLine("Sorry!! You don't have enough balance in your account");
                int choice = commonMethods.ValidateInt("Do you want to continue >> 1. Yes 2. No : ");
                if (choice == 1) WithDraw();
                else furtherAction();
            }

            Transaction transaction = new Transaction() { Id= "TXN" + LoginMethods.CurrentBank.Id + LoginMethods.CurrentAccount.Id + DateTime.Now,
                Type="Withdraw",
                Amount=amount,
                FromAccount= LoginMethods.CurrentAccount.Number,
                ToAccount= LoginMethods.CurrentAccount.Number,
                Date= DateTime.Now,
                PostBalance= LoginMethods.CurrentAccount.Balance,
                Charge=0
            };
           
            transactionService.AddTransaction(transaction);

            Console.WriteLine("Withdrawal successfull!!");
            Console.WriteLine("Your transaction id is : " + transaction.Id);
            Console.WriteLine("Your current balance is : " + transaction.PostBalance);
            furtherAction();
        }

        //Deposit 
        public void Deposit()
        {
            if(LoginMethods.CurrentAccount==null) CheckAccount();
            Console.WriteLine("Enter the currency type you want to deposit : ");
            int optionNumber = 1;

            List<Currency> currencies = currencyService.GetAllCurrencies();

            foreach (Currency c in currencies)
            {
                Console.WriteLine(optionNumber + ". " + c.Type);
                optionNumber += 1;
            }

            int choice = commonMethods.ValidateInt("Enter your choice : ");
            Currency currency = currencies[choice-1];

            if (currency == null)
            {
                Console.WriteLine("Please enter valid currency type");
                Deposit();
            }
            double exchangeRate = currency.ExchangeRate;

            double amount = commonMethods.ValidateDouble("Enter the amount you want to deposit : ");
            double balance = LoginMethods.CurrentAccount.Balance;
            LoginMethods.CurrentAccount.Balance = balance + amount;

            Transaction transaction = new Transaction() {
                Id = "TXN" + LoginMethods.CurrentBank.Id + LoginMethods.CurrentAccount.Id + DateTime.Now,
                Type = "Deposit",
                Amount = amount,
                FromAccount = LoginMethods.CurrentAccount.Number,
                ToAccount = LoginMethods.CurrentAccount.Number,
                Date = DateTime.Now,
                PostBalance = LoginMethods.CurrentAccount.Balance,
                Charge = amount * exchangeRate
            };

            transactionService.AddTransaction(transaction);

            Console.WriteLine("Deposit successfull!!");
            Console.WriteLine("Your transaction id is : " + transaction.Id);
            Console.WriteLine("Your current balance is : " + transaction.PostBalance);
            furtherAction();
        }

        //Transfer
        public void Transfer()
        {
            double charge = 0;
            string recepientBankName = commonMethods.ReadString("Enter the bank name of recepient : ");
            Bank recepientBank = bankService.GetBankByName(recepientBankName.ToLower());
            while(recepientBank==null)
            {
                Console.WriteLine("Sorry!! We could not find any bank with the given name");
                int Choice = commonMethods.ValidateInt("1. Please enter a valid bank name 2. Back >> ");
                if (Choice == 1)
                {
                    recepientBankName = Console.ReadLine();
                    recepientBank = bankService.GetBankByName(recepientBankName.ToLower());
                }
                else furtherAction();
            }

            string recepientName = commonMethods.ReadString("Enter the name of the recepient : ");
            List<Account> recepientAccounts = accountService.GetAllAccounts(recepientName, recepientBank.Id);

            while (recepientAccounts.Count==0)
            {
                Console.WriteLine("Sorry!! We could not find any account linked to the given name");
                int decision = commonMethods.ValidateInt("1. Please enter a valid recepient name 2. Back : ");
                if (decision == 1)
                {
                    recepientName = Console.ReadLine();
                    recepientAccounts = accountService.GetAllAccounts(recepientName, recepientBank.Id);
                }
                else furtherAction();
            }

            Console.Write("Enter the account you want to transfer to : ");
            int i = 1;

            foreach (Account account in recepientAccounts)
            {
                Console.Write(i + " " + Enum.GetName(typeof(AccountType), account.Type) + " ");
                i += 1;
            }
            int choice = commonMethods.ValidateInt("Please enter your account choice : ");
            while ((!recepientAccounts.Exists(obj => Convert.ToInt32(obj.Type) == choice)) || choice > recepientAccounts.Count || choice < 0)
            {
                Console.Write("Please enter a valid choice : ");
                choice = Int32.Parse(Console.ReadLine());
            }
            Account recepientAccount = recepientAccounts[choice - 1];

            double amount = commonMethods.ValidateDouble("Enter the amount to transfer : ");
            if(LoginMethods.CurrentAccount==null)
            {
                Console.Write("Enter the account you want to transfer from : ");
                i = 1;

                List<Account> senderAccounts = accountService.GetAllAccountsByName(LoginMethods.CurrentUser.UserName);
                foreach (Account account in senderAccounts)
                {
                    Console.Write(i + " " + Enum.GetName(typeof(AccountType), account.Type) + " ");
                    i += 1;
                }
                choice = commonMethods.ValidateInt("Please enter your account choice : ");
                while ((!recepientAccounts.Exists(obj => Convert.ToInt32(obj.Type) == choice)) || choice > recepientAccounts.Count || choice < 0)
                {
                    choice = commonMethods.ValidateInt("Please enter a valid choice : ");
                }
                LoginMethods.CurrentAccount = senderAccounts[choice - 1];
            }
            Bank bank = bankService.GetBankById(LoginMethods.CurrentAccount.BankId);

            if (LoginMethods.CurrentAccount.Balance < amount)
            {
                Console.WriteLine("Sorry!! You don't have enough balance to transfer..");
                furtherAction();
            }

            if (amount < 200000)
            {
                charge = amount * bank.RTGS_Charges;
            }
            else
            {
                charge = amount * bank.IMPS_Charges;
            }

            LoginMethods.CurrentAccount.Balance = LoginMethods.CurrentAccount.Balance - amount - charge;

            recepientAccount.Balance = recepientAccount.Balance + amount;

            string bankId = LoginMethods.CurrentAccount.BankId;
            Transaction transaction = new Transaction() {
                Id = "TXN" + bankId + LoginMethods.CurrentAccount.Id + DateTime.Now,
                Type="Transfer",
                Amount=amount,
                FromAccount = LoginMethods.CurrentAccount.Number,
                ToAccount = recepientAccount.Number,
                Date = DateTime.Now,
                PostBalance = LoginMethods.CurrentAccount.Balance,
                Charge = charge,
            };

            transactionService.AddTransaction(transaction);
            Console.WriteLine("Transferred successfully!!");
            Console.WriteLine("Your transaction id is : " + transaction.Id);
            Console.WriteLine("Your current balance is : " + transaction.PostBalance);
            
            furtherAction();
        }

        //Revert a transaction
        public void RevertTransaction()
        {
            string id = commonMethods.ReadString("Enter the transaction id you want to revert : "); 
            Transaction transaction = transactionService.GetTransaction(id);
            while(transaction==null)
            {
                int choice = commonMethods.ValidateInt("1. Please enter a valid transaction id 2.Exit : ");
                if (choice == 2) furtherAction();
                else id = commonMethods.ReadString("Transaction Id : ");
                transaction = transactionService.GetTransaction(id);
            }

            string toAccount = transaction.ToAccount;
            Account to = accountService.GetAccountByNumber(toAccount);
            to.Balance = to.Balance - transaction.Amount;
            if(to.Balance<0)
            {
                Console.WriteLine("Sorry!! The transaction cannot be reverted..");
                furtherAction();
            }

            string fromAccount = transaction.FromAccount;
            Account from = accountService.GetAccountByNumber(fromAccount);
            from.Balance = from.Balance + transaction.Amount;

            string bankId = from.BankId;
            Transaction newTransaction = new Transaction()
            {
                Id = "TXN" + bankId + to.Id + DateTime.Now,
                Type = "Revert | Withdraw",
                Amount = transaction.Amount,
                FromAccount = to.Number,
                ToAccount = from.Number,
                Date = DateTime.Now,
                PostBalance = to.Balance,
                Charge = 0
            };
            transactionService.AddTransaction(transaction);

            newTransaction = new Transaction()
            {
                Id = "TXN" + bankId + from.Id + DateTime.Now,
                Type = "Revert | Deposit",
                Amount = transaction.Amount,
                FromAccount = to.Number,
                ToAccount = from.Number,
                Date = DateTime.Now,
                PostBalance = from.Balance,
                Charge = 0
            };
            transactionService.AddTransaction(transaction);

            Console.WriteLine("Transferred successfully!!");
            Console.WriteLine("Transaction reverted successfully");
            furtherAction();
        }

        //View transaction history
        public void ViewHistory()
        {
            List<Transaction> transactions = transactionService.ViewHistory();

            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine("Id : " + transaction.Id + "From Account : " + transaction.FromAccount + " To Account : " + transaction.ToAccount + " Transaction Date : " + transaction.Date + " Amount : " + transaction.Amount
                    + " Charge : " + transaction.Charge + " Post Balance : " + transaction.PostBalance);
            }

            Console.WriteLine("Total Transaction Count : " + transactions.Count);
            furtherAction();
        }

        public void furtherAction()
        {
            IUserActions userActions = new UserActions();
            string decision = commonMethods.ReadString("Do you want to continue >> 1. Yes 2. No : ");
            Console.Clear();
            if (decision == "Yes" || decision == "Y" || decision == "yes" || decision == "1")
            {
                if (LoginMethods.CurrentUser.Type == "staff")
                {
                    StaffActions staff = new StaffActions();
                    staff.DisplayMethods();
                }
                else userActions.SelectAction();
            }
            else
            {
                Console.WriteLine("You are logged out");
                login.ValidateUser();
            }
        }
    }
}
