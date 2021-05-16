using BoxesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Communicate myBox = new Communicate();
            ConfigurationData configuration = new ConfigurationData(1000, 20, 2, 3, 200, 1, 30); //Demo configuration
            Manager manager = new Manager(myBox, configuration); //Initiazling the manager
            try //Initializing boxes data
            {
                manager.AddSupply(15, 17, 100);
                manager.AddSupply(25, 30, 50);
                manager.AddSupply(25, 27, 50);
                manager.AddSupply(20, 25, 70);
                manager.AddSupply(10, 15, 150);
                manager.AddSupply(5, 10, 150);
                manager.AddSupply(7, 15, 250);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }

            UserInterface(manager); //Initializing the user interface of the application
        }

        static void UserInterface(Manager manager)
        {
            int option = default, count = default;
            double width = default, height = default;
            bool toContinue = false;

            do
            {
                PrintMenu(); //Printing the menu
                if (!int.TryParse(Console.ReadLine(), out option)) //Asking the user to pick an option from the list and checking it is a digit
                {
                    InvalidInput();
                    continue;
                }

                switch (option)
                {
                    case 1: //Adding supply
                        Console.WriteLine("Please enter width, height and then the quantity of the box you wanna add supply to");
                        if (!double.TryParse(Console.ReadLine(), out width) || !double.TryParse(Console.ReadLine(), out height) ||
                            !int.TryParse(Console.ReadLine(), out count))
                        {
                            InvalidInput();
                            break;
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        try
                        {
                            if (manager.AddSupply(width, height, count)) Console.WriteLine("Supply was added successfuly\n");
                            else Console.WriteLine($"The box {width} X {height} was added to the system\n");
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{e.Message}\n");
                        }
                        break;

                    case 2: //Getting details
                        Console.WriteLine("Please enter the width and then the height of the box you wanna get details of");
                        if (!double.TryParse(Console.ReadLine(), out width) || !double.TryParse(Console.ReadLine(), out height))
                        {
                            InvalidInput();
                            break;
                        }

                        try
                        {
                            if (!manager.GetDetails(width, height))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("There isn't a box with those values in the system\n");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{e.Message}\n");
                        }
                        break;

                    case 3: //Finding a box for a gift
                        Console.WriteLine("Please enter the width and height of the gift that you need boxes of and then the required quantity");
                        if (!double.TryParse(Console.ReadLine(), out width) || !double.TryParse(Console.ReadLine(), out height) ||
                            !int.TryParse(Console.ReadLine(), out count))
                        {
                            InvalidInput();
                            break;
                        }
                        try
                        {
                            if (!manager.FindBestMatch(width, height, count)) Console.WriteLine("Sorry, try different values\n");
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{e.Message}\n");
                        }
                        break;

                    case 4: //Clearing the console window
                        Console.Clear();
                        toContinue = true; //The user just wanna clear the console and choose an option from the list
                        continue;

                    case 5: //Quit from the application
                        break;

                    default:
                        InvalidInput();
                        break;
                }

                if (option != 5) toContinue = GetInputAfterAction(); //Checking if the user wanna do something else (except when he wanna quit)
            }
            while (option != 5 && toContinue == true);

            Console.WriteLine("Thank you, have a nice day");
        }

        private static bool GetInputAfterAction() //Checking if the user wanna continue or not
        {
            string inputAfter;
            Console.ForegroundColor = ConsoleColor.Yellow;

            do
            {
                Console.WriteLine("Do you wanna do something else?\nYes/No?");
                inputAfter = Console.ReadLine();
            } while (inputAfter.ToLower() != "yes" && inputAfter.ToLower() != "no");

            Console.WriteLine("-------------------------------------------------------");
            if (inputAfter.ToLower() == "no") return false;
            return true;
        }

        private static void InvalidInput() //Printing a failure message
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("One of your inputs is invalid\n");
        } 

        private static void PrintMenu() //Printing the UI options menu
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Hello User,\nPlease choose an action from the list below:");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("1 - for adding supply of a box");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("2 - for getting details of a box");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("3 - for finding a box for a gift");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("4 - for clearing the console");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("5 - for quitting from the application");
            Console.WriteLine("-------------------------------------------------------");
            Console.Write("Your action is: ");
        }
    }
}
