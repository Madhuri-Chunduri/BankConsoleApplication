using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface IBankService
    {
        void AddBank(Bank bank);

        Bank GetBankByName(string bankName);

        Bank GetBankById(string id);

        List<Bank> GetAllBanks();
    }
}
