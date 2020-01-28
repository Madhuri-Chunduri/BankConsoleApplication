using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    class BankService : IBankService
    {
        public static List<Bank> Banks = new List<Bank>();

        public void AddBank(Bank bank)
        {
            Banks.Add(bank);
        }

        public Bank GetBankByName(string bankName)
        {
            return Banks.Find(obj => obj.Name == bankName);
        }

        public Bank GetBankById(string id)
        {
            return Banks.Find(obj => obj.Id == id);
        }

        public List<Bank> GetAllBanks()
        {
            return Banks;
        }

    }
}
