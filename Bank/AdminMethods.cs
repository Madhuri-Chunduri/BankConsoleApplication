using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    class AdminMethods
    {
        CommonMethods commonMethods = new CommonMethods();
        public void AdminActions()
        {
            Console.WriteLine("1. Add new bank ");
            Console.WriteLine("2. View all banks ");
            Console.WriteLine("3. Logout");

            int choice = commonMethods.ValidateInt("Your choice : ");
            switch (choice)
            {
                case 1:
                    BankSetup bankSetup = new BankSetup();
                    bankSetup.AddNewBank();
                    AdminActions();
                    break;

                case 2:
                    IBankService bankService = new BankService();
                    List<Bank> banks = bankService.GetAllBanks();
                    foreach (Bank b in banks)
                    {
                        Console.WriteLine("Bank Id : " + b.Id + " Bank Name : " + b.Name);
                    }
                    Console.WriteLine("Total Banks Count : " + banks.Count);
                    AdminActions();
                    break;

                case 3: LoginMethods loginMethods = new LoginMethods();
                    loginMethods.ValidateUser();
                    break;

                default:
                    Console.Write("Please enter a valid choice : ");
                    break;

            }

        }
    }
}
