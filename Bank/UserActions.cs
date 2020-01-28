using Bank.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class UserActions : IUserActions
    {
        TransactionMethod transactionMethod = new TransactionMethod();
        LoginMethods login = new LoginMethods();
        UserService userService = new UserService();
        ValidationMethods validationMethods = new ValidationMethods();
        CommonMethods commonMethods = new CommonMethods();

        public void DisplayMethods()
        {
            Console.WriteLine("Enter the action you want to perform");

            Console.WriteLine("1. Withdraw Amount ");
            Console.WriteLine("2. Deposit Amount ");
            Console.WriteLine("3. Transfer Funds");
            Console.WriteLine("4. View Transaction History ");
            Console.WriteLine("5. Update Profile");
            Console.WriteLine("6. LogOut");
        }

        public void SelectAction()
        {
            DisplayMethods();
            int choice= commonMethods.ValidateInt("Your Choice Number : ");
            while(choice<1 || choice>6)
            {
                choice = commonMethods.ValidateInt("Please enter a valid choice between 1 and 6 : ");
            }
            TakeAction(choice);
        }

        public void TakeAction(int choice)
        { 
            switch(choice)
            {
                case (1): transactionMethod.WithDraw();
                            break;

                case (2): transactionMethod.Deposit();
                            break;

                case (3): transactionMethod.Transfer();
                            break;

                case (4): transactionMethod.ViewHistory();
                            break;

                case (5): UpdateProfile();
                            break;

                case (6): Logout();
                            break;

            }

        }
       
        public void UpdateProfile()
        {
            Console.WriteLine("Enter the field you want to update : ");
            int choice = commonMethods.ValidateInt("1. Name 2. Mobile Number 3. Address 4. Email 5. Password 6.Exit : ");
            User user = LoginMethods.CurrentUser;
            User updatedUser = new User();
            switch(choice)
            {
                case 1: Console.WriteLine("Your old name : "+ user.Name);
                        user.Name = commonMethods.ReadString("Please enter new name : ");
                        while(user.Name=="")
                        {
                            Console.WriteLine("Please enter a valid name");
                            user.Name = commonMethods.ReadString("Enter new name : ");
                        }
                        updatedUser=userService.UpdateUser(user);
                        break;
                case 2: Console.WriteLine("Your old mobile number : " + user.MobileNumber);
                        user.MobileNumber = commonMethods.ReadString("Please enter new mobile number : ");
                        bool validNumber = validationMethods.IsValidPhoneNumber(user.MobileNumber);
                        while (validNumber != true)
                        {
                            Console.WriteLine("Sorry!! This is not a valid mobile number..");
                            user.MobileNumber = commonMethods.ReadString("Enter valid Mobile Number :");
                            validNumber = validationMethods.IsValidPhoneNumber(user.MobileNumber);
                        }
                        updatedUser = userService.UpdateUser(user);
                        break;
                case 3: Console.WriteLine("Your old address : " + user.Address);
                        user.Address = commonMethods.ReadString("Please enter new address : ");
                        updatedUser = userService.UpdateUser(user);
                        break;
                case 4: Console.WriteLine("Your old email : " + user.Email);
                        user.Email = commonMethods.ReadString("Please enter new email : ");
                        bool validEmail = validationMethods.IsValidEmail(user.Email);
                        while (validEmail == false)
                        {
                            user.Email = commonMethods.ReadString("Enter valid Email Address : ");
                            validEmail = validationMethods.IsValidEmail(user.Email);
                        }
                        updatedUser = userService.UpdateUser(user);
                        break;
                case 5: Console.WriteLine("Your old password : " + user.Password);
                        user.Password = commonMethods.ReadString("Please enter new password : ");
                        while (user.Password=="")
                        {
                            user.Password = commonMethods.ReadString("Please enter a valid password : ");
                        }
                        updatedUser = userService.UpdateUser(user);
                        break;
                case 6: furtherAction();
                        break;
               default: Console.WriteLine("Please enter a valid choice");
                        UpdateProfile();
                        break;
            }
            Console.WriteLine("Your current details are : ");
            Console.WriteLine(" Name : " + updatedUser.Name);
            Console.WriteLine(" Mobile Number : " + updatedUser.MobileNumber);
            Console.WriteLine(" Address : " + updatedUser.Address);
            Console.WriteLine(" Email : " + updatedUser.Email);
            Console.WriteLine(" Password : " + updatedUser.Password);
            furtherAction();
        }

        public void Logout()
        {
            Console.Clear();
            LoginMethods.CurrentUser = null;
            LoginMethods.CurrentBank = null;
            LoginMethods.CurrentAccount = null;
            Console.WriteLine("Logged Out successfully");
            login.ValidateUser();
        }

        public void furtherAction()
        {
            Console.Write("Do you want to continue >> 1. Yes 2. No : ");
            int decision = Int32.Parse(Console.ReadLine());
            Console.Clear();
            switch(decision)
            {
                case 1: if (LoginMethods.CurrentUser.Type == "staff")
                        {
                             StaffActions staff = new StaffActions();
                             staff.DisplayMethods();
                        }
                        else SelectAction();
                        break;

                case 2: Console.WriteLine("You are logged out");
                        login.ValidateUser();
                        break;
                default: Console.WriteLine("Invalid choice!!!");
                         furtherAction();
                         break;
            }
        }
    }
}
