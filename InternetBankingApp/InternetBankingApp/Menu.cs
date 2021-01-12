using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Services;
using InternetBankingApp.Utilities;
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
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private Customer _loggedInCustomer;
        public Menu(LoginService loginService, CustomerService customerService, AccountService accountService)
        {
            _loginService = loginService;
            _customerService = customerService;
            _accountService = accountService;
            DisplayLogin();
        }

        public void DisplayMainMenu()
        {
            while (true)
            {
                // TODO replace dude with name
                Console.Write(
 @$"----- Main Menu ----
Welcome {_loggedInCustomer.Name}

Please select an option from the following:
   
1. ATM Transaction
2. Transfer
3. My Statements
4. Logout
5. Exit
Enter an option: ");

                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out int option) || !(option is >= 1 and <= 5))
                {
                    Console.WriteLine("Invalid input. Please re-enter your selection.");
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
                var passwordInput = GetPasswordFromInput();
                Console.WriteLine();
                if (_loginService.AuthenticateUser(loginIDInput, passwordInput))
                {
                    var customerID = _loginService.GetCustomerIDFromLogin(loginIDInput);
                    _loggedInCustomer = _customerService.GetCustomer(customerID);
                    DisplayMainMenu();
                }
                else
                {
                    Console.WriteLine("Incorrect loginID or password\n");
                    continue;
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
                        DisplayAccounts();
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
                Account savingsAcc = null;
                Account checkingAcc = null;
                if (_loggedInCustomer.HasSavingsAccount())
                {
                    savingsAcc = _accountService.GetAccount("S", _loggedInCustomer);
                }
                else
                {
                    checkingAcc = _accountService.GetAccount("C", _loggedInCustomer);
                }

                Console.Write(
@$"--- Select Account ---

Select an account to deposit money into:

1. Savings Account - {(savingsAcc != null ? savingsAcc.Balance : "Unavailable")}
2. Checking Account - {(checkingAcc != null ? checkingAcc.Balance : "Unavailable")}
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
                        // DepositSavings
                        if (_loggedInCustomer.HasSavingsAccount())
                        {
                            Deposit(savingsAcc);
                        }
                        else
                        {
                            Console.WriteLine("You do not have any savings account.");
                            continue;
                        }
                        break;
                    case 2:
                        // DepositChecking()
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void Deposit(Account account)
        {
            while (true)
            {
                Console.Write(
@$"--- Deposit Amount ---

Your available balance is ${account.Balance}

Enter the amount you would like to deposit, or press enter to return : $");


            }
        }

        public void DepositSavings()
        {
            // Show balance
        }

        public void DisplayBalance()
        {

        }


        public void DisplayDeposit()
        {
            Console.Write(
@"--- Deposit Amount ----

Your available balance is ${}

Enter the amount you would like to deposit, or press enter to return : $");

            var input = Console.ReadLine();
            Console.WriteLine();

            
        }

        /// <summary>
        /// Hides users inputted keys.
        /// Referenced from : https://dotnetcodr.com/2015/09/02/how-to-hide-the-text-entered-in-a-net-console-application/
        /// </summary>
        /// <returns>Hidden string input.</returns>
        private static string GetPasswordFromInput()
        {
            var passwordBuilder = new StringBuilder();
            bool continueReading = true;
            char newLineChar = '\r';
            while (continueReading)
            {
                ConsoleKeyInfo consoleKey = Console.ReadKey(true);
                char passwordChar = consoleKey.KeyChar;

                if (passwordChar == newLineChar)
                {
                    continueReading = false;
                }
                else
                {
                    passwordBuilder.Append(passwordChar.ToString());
                }
            }
            return passwordBuilder.ToString();
        }

        private bool ValidateMenuInput(string input, int maxRange)
        {
            if (!int.TryParse(input, out int option) || !option.IsInRange(1, maxRange))
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine();
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateDepositInput(string input)
        {
            if (!int.TryParse(input, out int option)){
                Console.WriteLine("Invalid input.")
                return false;
            }
        }

    }
}
