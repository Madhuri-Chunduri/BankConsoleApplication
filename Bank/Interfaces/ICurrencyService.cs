using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface ICurrencyService
    {
        void AddCurrency(Currency currency);

        List<Currency> GetAllCurrencies();

        Currency GetCurrency(string type);

        void UpdateCurrency(string type, double exchangeRate);
    }
}
