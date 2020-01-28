using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    public class TransactionService : ITransactionService
    {
        public static List<Transaction> Transactions = new List<Transaction>();
        IAccountService accountService = new AccountService();

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
       
        //Revert a transaction
        public int RevertTransaction(string id)
        {
            Transaction transaction = Transactions.Find(obj=> obj.Id == id);
            if (transaction == null) return 0;

            string fromAccount = transaction.FromAccount;
            Account from = accountService.GetAccountByNumber(fromAccount);
            from.Balance = from.Balance + transaction.Amount;

            string toAccount = transaction.ToAccount;
            Account to = accountService.GetAccountByNumber(toAccount);
            to.Balance = to.Balance - transaction.Amount;
            return 1;
        }

        public Transaction GetTransaction(string id)
        {
            return Transactions.Find(obj => obj.Id == id);
        }

        //View transaction history
        public List<Transaction> ViewHistory()
        {
            List<Transaction> requiredTransactions = new List<Transaction>();
            //Account account = accountService.GetAccountByName(LoginMethods.currentUser.UserName);
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.FromAccount == LoginMethods.CurrentAccount.Number || transaction.ToAccount == LoginMethods.CurrentAccount.Number)
                {
                    Account fromAccount = accountService.GetAccountByNumber(transaction.FromAccount);
                    Account toAccount = accountService.GetAccountByNumber(transaction.ToAccount);
                    if (fromAccount == null || toAccount == null)
                    {
                        continue;
                    }
                    if(fromAccount.BankId == LoginMethods.CurrentAccount.BankId || toAccount.BankId == LoginMethods.CurrentAccount.BankId)
                        requiredTransactions.Add(transaction);
                }
            }
            return requiredTransactions;
        }
    }
}
