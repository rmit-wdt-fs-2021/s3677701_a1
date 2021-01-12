using InternetBankingApp.Exceptions;
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
                        DisplayTransferMenu();
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

        private void DisplayTransferMenu()
        {
            Account savingsAcc = null;
            Account checkingAcc = null;
            if (_loggedInCustomer.HasSavingsAccount())
            {
                savingsAcc = _accountService.GetAccount("S", _loggedInCustomer);
            }
            if (_loggedInCustomer.HasCheckingAccount())
            {
                checkingAcc = _accountService.GetAccount("C", _loggedInCustomer);
            }

            while (true)
            {
                AccountSelectionMenu("Select an account to transfer money from", savingsAcc, checkingAcc);
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

        private void DisplayATMMenu()
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
                        DisplayAccountsForDeposit();
                        break;
                    case 2:
                        DisplayAccountsForWithdrawal();
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void DisplayAccountsForWithdrawal()
        {
            while (true)
            {
                Account savingsAcc = null;
                Account checkingAcc = null;
                if (_loggedInCustomer.HasSavingsAccount())
                {
                    savingsAcc = _accountService.GetAccount("S", _loggedInCustomer);
                }
                if (_loggedInCustomer.HasCheckingAccount())
                {
                    checkingAcc = _accountService.GetAccount("C", _loggedInCustomer);
                }

                Console.Write(
@$"--- Select Account ---

Select an account to withdraw money from:

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
                            Withdraw(savingsAcc);
                        }
                        else
                        {
                            Console.WriteLine("You do not have a savings account.");
                            continue;
                        }
                        break;
                    case 2:
                        // DepositChecking()
                        if (_loggedInCustomer.HasCheckingAccount())
                        {
                            Withdraw(checkingAcc);
                        }
                        else
                        {
                            Console.WriteLine("You do not have a checking account.");
                            continue;
                        }
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void AccountSelectionMenu(string message, Account savingsAcc, Account checkingAcc)
        {
            Console.Write(
@$"--- Select Account ---

{message}:

1. Savings Account - {(savingsAcc != null ? savingsAcc.Balance : "Unavailable")}
2. Checking Account - {(checkingAcc != null ? checkingAcc.Balance : "Unavailable")}
3. Return to Main Menu");
        }

        private void DisplayAccountsForDeposit()
        {
            Account savingsAcc = null;
            Account checkingAcc = null;
            if (_loggedInCustomer.HasSavingsAccount())
            {
                savingsAcc = _accountService.GetAccount("S", _loggedInCustomer);
            }
            if (_loggedInCustomer.HasCheckingAccount())
            {
                checkingAcc = _accountService.GetAccount("C", _loggedInCustomer);
            }

            while (true)
            {
                AccountSelectionMenu("Select an account to deposit money to", savingsAcc, checkingAcc);

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
                            Console.WriteLine("You do not have a savings account.");
                            continue;
                        }
                        break;
                    case 2:
                        // DepositChecking()
                        if (_loggedInCustomer.HasCheckingAccount())
                        {
                            Deposit(checkingAcc);
                        }
                        else
                        {
                            Console.WriteLine("You do not have a checking account.");
                            continue;
                        }
                        break;
                    case 3:
                        DisplayMainMenu();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void Withdraw(Account account)
        {
            while (true)
            {
                Console.Write(
@$"--- Withdraw Amount ---

Your available balance is ${account.Balance}

Enter the amount you would like to withdraw, or press enter to return : $");

                var input = Console.ReadLine();
                Console.WriteLine();
                if (input == string.Empty)
                {
                    DisplayAccountsForWithdrawal();
                }

                if (!ValidateMoneyInput(input))
                {
                    Console.WriteLine($"Withdrawal of {input} failed.\nPress any key to continue\n");
                    Console.ReadKey();
                    DisplayAccountsForWithdrawal();
                }
                else
                {
                    try
                    {
                        _accountService.DeductBalanceAsync(account, decimal.Parse(input)).Wait();
                        Console.WriteLine($"Withdrawal of ${input} was succesful");
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine($"Withdrawing ${input} would overdraw the account. " +
                            $"Checking account must have a minimum balance of $200.");
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Console.WriteLine($"Your balance is now {account.Balance}");
                    Console.WriteLine("Press any key to return to account selection menu.\n");
                    Console.ReadKey();
                    DisplayAccountsForWithdrawal();
                    break;
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


                var input = Console.ReadLine();
                Console.WriteLine();

                if (!ValidateMoneyInput(input))
                {
                    Console.WriteLine($"Deposit of {input} failed.\nPress any key to continue\n");
                    Console.ReadKey();
                    DisplayAccountsForDeposit();
                }
                else
                {
                    // Update DB
                    _accountService.AddBalanceAsync(account, balance: decimal.Parse(input)).Wait();

                    Console.WriteLine($"Deposit of ${input} was succesful");
                    Console.WriteLine($"Your balance is now {account.Balance}");
                    Console.WriteLine("Press any key to return to account selection menu.\n");
                    Console.ReadKey();
                    DisplayAccountsForDeposit();
                    break;
                }

            }
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

        private bool ValidateMoneyInput(string input)
        {
            bool retVal;
            if (!decimal.TryParse(input, out decimal option))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.WriteLine();
                retVal = false;
            }
            else
            {
                if (option > 0)
                {
                    retVal = true;
                }
                else
                {
                    Console.WriteLine("Cannot deposit 0 or negative number");
                    retVal = false;
                }
            }

            return retVal;
        }


    }

}
