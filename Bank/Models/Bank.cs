using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Bank
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double RTGS_Charges { get; set; }

        public double IMPS_Charges { get; set; }

        public double Interbank_RTGS_Charges { get; set; }

        public double Interbank_IMPS_Charges { get; set; }

        public List<Currency> Currency = new List<Currency> ();
    }
}
