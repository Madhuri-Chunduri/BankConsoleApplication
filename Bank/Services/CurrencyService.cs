
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank.Services
{
    class CurrencyService : ICurrencyService
    {
        public static List<Currency> Currencies = new List<Currency>();

        public void AddCurrency(Currency currency)
        {
            Currencies.Add(currency);
        }

        public List<Currency> GetAllCurrencies()
        {
            return Currencies.FindAll(obj => (obj.BankId == LoginMethods.CurrentBank.Id));
        }

        public Currency GetCurrency(String type)
        {
            return Currencies.Find(obj => obj.Type == type);
        }

        public void UpdateCurrency(string type,double exchangeRate)
        {
            Currencies.Where(obj => (obj.Type == type)).Select(obj => { obj.ExchangeRate = exchangeRate ; return obj; });
        }
    }
}
