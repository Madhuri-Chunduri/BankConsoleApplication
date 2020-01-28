using System;
using System.Collections.Generic;
using System.Text;
using Bank.Models;

namespace Bank
{
    public class Account
    {
        public Guid Id { get; set; }

        public string BankId { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        public double Balance { get; set; }

        public AccountType Type { get; set; }
    }
}
