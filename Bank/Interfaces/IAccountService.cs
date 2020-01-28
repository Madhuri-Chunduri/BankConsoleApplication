using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface IAccountService
    {
         void CreateAccount(Account account);

         void DeleteAccount(Account account);

        Account GetAccountByName(string userName);

        Account GetAccountById(string id);

        Account GetAccountByNumber(string number);

        List<Account> GetAllAccounts(string userName, string bankId);

        List<Account> GetAllAccountsByName(string userName);
    }
}
