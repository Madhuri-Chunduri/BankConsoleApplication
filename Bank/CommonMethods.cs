using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class CommonMethods
    {
        public string ReadString(string label)
        {
            Console.Write(label);
            string value = Console.ReadLine();
            return value;
        }

        public int ValidateInt(string message)
        {
            Console.Write(message);
            int value = 0;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Please enter a valid value : ");
            }
            return value;
        }

        public double ValidateDouble(string message)
        {
            Console.Write(message);
            double value = 0;
            while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Please enter a valid value : ");
            }
            return value;
        }
    }
}
