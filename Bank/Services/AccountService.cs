using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    public class AccountService : IAccountService
    {
        public static List<Account> Accounts = new List<Account>();

        //Create Account
        public void CreateAccount(Account account)
        {
            Accounts.Add(account);
        }

        //Delete Account
        public void DeleteAccount(Account account)
        {
            Accounts.Remove(account);
        }

        public Account GetAccountByName(string userName)
        {
            return Accounts.Find(obj => (obj.UserName == userName && obj.BankId==LoginMethods.CurrentBank.Id));
        }

        public Account GetAccountById(string id)
        {
            return Accounts.Find(obj => obj.Id.ToString() == id);
        }

        public List<Account> GetAllAccounts(string userName,string id)
        {
            return Accounts.FindAll(obj => (obj.UserName == userName && obj.BankId == id));
        }

        public Account GetAccountByNumber(string number)
        {
            return Accounts.Find(obj => obj.Number == number && obj.BankId==LoginMethods.CurrentBank.Id);
        }

        public List<Account> GetAllAccountsByName(string userName)
        {
            return Accounts.FindAll(obj => obj.UserName == userName && obj.BankId == LoginMethods.CurrentBank.Id);
        }
    }
}
