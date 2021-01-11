using InternetBankingApp.Interfaces;
using InternetBankingApp.Services;
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
        private readonly ILoginService _loginService;
        public Menu(LoginService loginService)
        {
            _loginService = loginService;
            DisplayLogin();
        }

        public void DisplayMainMenu()
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
                        DisplayATMMenu();
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
                        Environment.Exit(0);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void DisplayLogin()
        {
            while (true)
            {
                Console.Write(
    @"--- Login Menu ---
Please enter your Login Id: ");

                var loginIDInput = Console.ReadLine();
                Console.WriteLine();

                Console.Write(
@"Please enter your password: ");
                var passwordInput = Console.ReadLine(); //TODO : HIDE!!!
                Console.WriteLine();
                if(_loginService.AuthenticateUser(loginIDInput, passwordInput))
                {
                    Console.Write("Logged in");
                    DisplayMainMenu();
                }
                else
                {
                    Console.Write("HACK");
                }

            }
        }

        public void DisplayATMMenu()
        {
            while (true)
            {
                Console.Write(
@"--- ATM Menu ---
Please select an option from the following:

1. Deposit Money
2. Withdraw Money
3. Return to Main Menu");


                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out int option) || !(option is >= 1 and <= 3))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        // Deposit
                        break;
                    case 2:
                        // Withdraw
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void DisplayAccounts()
        {
            while (true)
            {
                Console.Write(
@"--- Select Account ---

Select an account to deposit money into:

1. Savings Account - {}
2. Checking Account - {}
3. Return to Main Menu");


                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out int option) || !(option is >= 1 and <= 3))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        // Deposit
                        break;
                    case 2:
                        // Withdraw
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }


        public void DisplayDeposit()
        {
            Console.Write(
@"--- Deposit Amount ----

Your available balance is ${}

Enter the amount you would like to deposit, or press enter to return : $");

        }

        // Get input

    }
}
