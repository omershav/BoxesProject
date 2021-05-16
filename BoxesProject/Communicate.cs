using BoxesLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesProject
{
    public class Communicate : ICommunicate
    {
        public void AlertMaxQuantity(int quantityNow, int countNotUsed) //Alerting when exceeded the max quantity
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"The quantity was updated to the max quantity of {quantityNow}, {countNotUsed} boxes will be returned back to the suppliers");
        }

        public void AlertMinQuantity(double width, double height, int count) //Alerting when we reached the min quantity
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You have to restock the size {width} X {height}, there are just {count} boxes left of this size.\n");
        }

        public bool DoesUserAgree() //Asking the user if he agrees to split his request to the size options are shown above
        {
            string temp;

            do
            {
                Console.WriteLine($"Do you agree to split your request to the options above?\nYes/No?");
                temp = Console.ReadLine();
            } while (temp.ToLower() != "yes" && temp.ToLower() != "no");

            return temp.ToLower() == "yes";
        }

        public void FailureMessage() //Printing a failure message when there isn't a suitable box for a gift
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("We couldn't find a suitable box for your gift in our system");
        }

        public bool GetUserAnswer(int count) //Asking the user if he wants to watch all the size options for his request (split list)
        {
            string temp;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Sorry, we don't have enough suitable boxes of this size. But, we can split your request to {count} different boxes size");
            Console.ForegroundColor = ConsoleColor.Yellow;

            do
            {
                Console.WriteLine("Do you wanna watch the options?\nYes/No?");
                temp = Console.ReadLine();
            } while (temp.ToLower() != "yes" && temp.ToLower() != "no");

            return temp.ToLower() == "yes";
        }

        public void PrintBoxData(double width, double height, int count) //Printing the box data
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Box {width} X {height}, quantity: {count}\n");
        }

        public void PrintBoxOptionDeleted(double width, double height) //Printing out of stock message
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"The box {width} X {height} is now out of stock and was deleted from the system\n");
        }

        public void PrintFullBoxData(double width, double height, int count, string date) //Printing the full box data
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The size {width} X {height} has quantity of {count} boxes and it was bought last time in: {date}\n");
        }

        public void SuccessMessage() //Printing a success message
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success! the data was updated in the system\n");
        }
    }
}
