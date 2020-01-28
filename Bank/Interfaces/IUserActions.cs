using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services
{
    interface IUserActions
    {
        void DisplayMethods();

        void TakeAction(int choice);

        void SelectAction();

        void UpdateProfile();

        void Logout();

        void furtherAction();
    }
}
