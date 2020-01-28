using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface ITransactionService
    {
        void AddTransaction(Transaction transaction);

        List<Transaction> ViewHistory();

        int RevertTransaction(string id);

        Transaction GetTransaction(string id);
    }
}
