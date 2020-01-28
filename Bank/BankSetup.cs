using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class BankSetup 
    {
        IBankService bankService = new BankService();
        ICurrencyService currencyService = new CurrencyService();
        IUserService userService = new UserService();
        LoginMethods login = new LoginMethods();

        public void AddNewBank()
        {
            Console.WriteLine("Please enter the new bank name");
            string bankName = Console.ReadLine();
            while(bankName=="")
            {
                Console.WriteLine("Bank Name cannot be empty");
                Console.Write("Please enter a valid bank name : ");
                bankName = Console.ReadLine();
            }

            Bank bank = new Bank()
            {
                Id = bankName.Substring(0, 3) + DateTime.Now,
                Name = bankName.ToLower(),
                RTGS_Charges = 0,
                IMPS_Charges = 0.05,
                Interbank_RTGS_Charges = 0.02,
                Interbank_IMPS_Charges = 0.06
            };
            Currency currency = new Currency() {
                Type = "rupees",
                ExchangeRate = 0,
                BankId = bank.Id
            };

            bankService.AddBank(bank);
            currencyService.AddCurrency(currency);

            User user = new User()
            {
                Name = "Manager",
                UserName = "manager",
                MobileNumber = "9999999999",
                Password = "manager",
                Type = "staff",
                Address = "Hyderabad",
                Email = "manager@technovert.net",
                BankId = bank.Id
            };
            userService.AddUser(user);
            Console.WriteLine("Bank " + bankName + " added successfully!!!");
            return; 
        }
    }
}
