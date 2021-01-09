using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp
{
    public enum MenuChoice
    {
        ATMTransactions = 1,
        Transfer,
        MyStatements,
        Logout,
        Exit
    }

    public class Menu
    {

        public void Run()
        {
            while (true)
            {
                // TODO replace dude with name
                Console.Write(
 @"----- Main Menu ----
   Welcome dude

   Please select an option from the following:
   
   1. ATM Transaction
   2. Transfer
   3. My Statements
   4. Logout
   5. Exit
   Enter an option: ");

                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out int option) || !(option is >=1 and <=5) )
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                
                MenuChoice choice = (MenuChoice)option;

                switch (choice)
                {
                    case MenuChoice.ATMTransactions:
                        Console.WriteLine("Displaying ATM");
                        break;
                    case MenuChoice.Transfer:
                        break;
                    case MenuChoice.MyStatements:
                        break;
                    case MenuChoice.Logout:
                        Console.WriteLine("Logging out..");
                        break;
                    case MenuChoice.Exit:
                        Console.WriteLine("Closing app..");
                        return;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

    }
}
