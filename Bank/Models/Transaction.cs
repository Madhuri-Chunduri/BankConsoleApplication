using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Transaction
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public double Amount { get; set; }

        public string FromAccount { get; set; }

        public string ToAccount { get; set; }

        public double Charge { get; set; }

        public DateTime Date { get; set; }

        public double PostBalance { get; set; }
    }
}
