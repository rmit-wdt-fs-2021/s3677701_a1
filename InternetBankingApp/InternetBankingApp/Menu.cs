using Authentication;
using Authentication.Interfaces;
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
        private readonly ITransactionService _transactionService;

        private Customer _loggedInCustomer;

        private const string UnavailableSavingsAcc = "You do not have a savings account.";
        private const string UnavailableCheckingAcc = "You do not have a checking account.";

        public Menu(LoginService loginService, CustomerService customerService, AccountService accountService, TransactionService transactionService)
        {
            _loginService = loginService;
            _customerService = customerService;
            _accountService = accountService;
            _transactionService = transactionService;
            DisplayLogin();
        }

        public void DisplayMainMenu()
        {
            while (true)
            {
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
                        DisplayStatementsMenu();
                        break;
                    case MenuChoice.Logout:
                        LogoutUser();
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

        private void LogoutUser()
        {
            Console.WriteLine($"Logging out {_loggedInCustomer.Name}");
            _loggedInCustomer = null;
            Console.Clear();
            DisplayLogin();
        }

        private void DisplayStatementsMenu()
        {
            while (true)
            {
                AccountSelectionMenu("Select an account to display the statement for", _loggedInCustomer.SavingsAccount, _loggedInCustomer.CheckingAccount);
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
                        if (_loggedInCustomer.HasSavingsAccount())
                        {
                            DisplayStatement(_loggedInCustomer.SavingsAccount);
                        }
                        else
                        {
                            Console.Write(UnavailableSavingsAcc);
                        }
                        break;
                    case 2:
                        if (_loggedInCustomer.HasCheckingAccount())
                        {
                            DisplayStatement(_loggedInCustomer.CheckingAccount);
                        }
                        else
                        {
                            Console.WriteLine(UnavailableCheckingAcc);
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

        private void DisplayStatement(Account account)
        {
            int skip = 0;
            while (true)
            {
                Console.WriteLine($"Account {account.AccountNumber} - Current balance: {account.Balance}\n");

                DisplayPagedStatementTable(account, skip);
                Console.WriteLine();
                Console.WriteLine("Press the right arrow => to view the next page of transactions.");
                Console.WriteLine("Press enter to return to the account selection.");
                Console.WriteLine();
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    AccountSelectionMenu("Select an account to display the statement for", _loggedInCustomer.SavingsAccount,
                        _loggedInCustomer.CheckingAccount);
                    break;

                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    skip += 4;
                    continue;
                }
            }
        }

        private void DisplayPagedStatementTable(Account account, int skip)
        {
            const string format = "{0,-20} | {1,-5} | {2,-15} | {3,-15} | {4,-15} | {5,-20} | {6}";
            Console.WriteLine(format, "Transaction Id", "Type", "From Account", "To Account", "Amount", "Comment", "Date");
            Console.WriteLine(new string('-', 135));
            foreach (var trans in _transactionService.GetPagedTransactions(account, 4, skip: skip))
            {
                Console.WriteLine(format, trans.TransactionID, trans.TransactionType, trans.AccountNumber, trans.DestinationAccountNumber,
                    trans.Amount.RoundUp(), trans.Comment, trans.TransactionTimeUtc.ToLocalTime());
            }

        }

        private void DisplayTransferMenu()
        {
            while (true)
            {
                AccountSelectionMenu("Select an account to transfer money from", _loggedInCustomer.SavingsAccount, _loggedInCustomer.CheckingAccount);

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
                        if (_loggedInCustomer.HasSavingsAccount())
                        {
                            Transfer(_loggedInCustomer.SavingsAccount);
                        }
                        else
                        {
                            Console.Write(UnavailableSavingsAcc);
                        }
                        break;
                    case 2:
                        if (_loggedInCustomer.HasCheckingAccount())
                        {
                            Transfer(_loggedInCustomer.CheckingAccount);
                        }
                        else
                        {
                            Console.WriteLine(UnavailableCheckingAcc);
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

        private void Transfer(Account srcAccount)
        {
            while (true) {
                Console.Write("Enter the account number you wish to transfer to, or press enter to return to the account selection menu : ");
                var input = Console.ReadLine();
                Console.WriteLine();
                if (input == string.Empty)
                {
                    DisplayTransferMenu();
                    break;
                }

                // Account number will always be 4 digits.
                if (!int.TryParse(input, out int accountNumber) || (input.Length != 4))
                {
                    Console.WriteLine("Invalid input. Please re-enter account number.");
                    Console.WriteLine();
                    continue;
                }

                if(accountNumber == srcAccount.AccountNumber)
                {
                    Console.WriteLine("Source and destination account number cannot be the same.");
                    continue;
                }

                Console.Write(
@$"--- Transfer Amount ---

Your available balance is ${srcAccount.Balance}

Enter the amount you would like to transfer to account {accountNumber}, or press enter to return to the account selection menu: $");

                var transferInput = Console.ReadLine();
                Console.WriteLine();
                if (transferInput == string.Empty)
                {
                    DisplayTransferMenu();
                    break;
                }

                if (!ValidateMoneyInput(transferInput))
                {
                    continue;
                }

                Console.Write("Please add a description (optional): ");
                var desc = Console.ReadLine();
                Console.WriteLine();
                // Update DB
                var amt = decimal.Parse(transferInput);
                const decimal transferCharge = 0.20M;
                try
                {
                    _transactionService.AddTransactionAsync("T", srcAccount, amt, DateTime.UtcNow, accountNumber, comment: desc).Wait();
                    _transactionService.AddTransactionAsync("S", srcAccount, transferCharge, DateTime.UtcNow).Wait();
                }catch(AggregateException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                DisplayBalance(srcAccount);
                Console.WriteLine("Press any key to return to the account selection menu.");
                Console.ReadKey();
                AccountSelectionMenu("Select an account to transfer money from", _loggedInCustomer.SavingsAccount, _loggedInCustomer.CheckingAccount);
                break;
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
                AccountSelectionMenu("Select an account to withdraw money from", _loggedInCustomer.SavingsAccount, _loggedInCustomer.CheckingAccount);

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
                        if (_loggedInCustomer.HasSavingsAccount())
                        {
                            Withdraw(_loggedInCustomer.SavingsAccount);
                        }
                        else
                        {
                            Console.WriteLine("You do not have a savings account.");
                            continue;
                        }
                        break;
                    case 2:
                        if (_loggedInCustomer.HasCheckingAccount())
                        {
                            Withdraw(_loggedInCustomer.CheckingAccount);
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
            Console.WriteLine(
@$"--- Select Account ---

{message}:

1. Savings Account - {(savingsAcc != null ? savingsAcc.AccountNumber : "Unavailable")}
2. Checking Account - {(checkingAcc != null ? checkingAcc.AccountNumber : "Unavailable")}
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
                        _transactionService.AddTransactionAsync("W", account, decimal.Parse(input), DateTime.UtcNow).Wait();
                        _transactionService.AddTransactionAsync("S", account, 0.10M, DateTime.UtcNow).Wait();

                        Console.WriteLine($"Withdrawal of ${input} was succesful");
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine("Withdrawal failed. Please try again.");
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something has gone wrong on our end. Please try again.");
                        Console.WriteLine(e.Message);
                    }

                    DisplayBalance(account);
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
                if (input == string.Empty)
                {
                    DisplayAccountsForDeposit();
                }

                if (!ValidateMoneyInput(input))
                {
                    Console.WriteLine($"Deposit of {input} failed.\nPress any key to continue\n");
                    Console.ReadKey();
                    DisplayAccountsForDeposit();
                }
                else
                {
                    // Update DB
                    _transactionService.AddTransactionAsync("D", account, decimal.Parse(input), DateTime.UtcNow).Wait();

                    Console.WriteLine($"Deposit of ${input} was succesful");
                    DisplayBalance(account);
                    Console.WriteLine("Press any key to return to account selection menu.\n");
                    Console.ReadKey();
                    DisplayAccountsForDeposit();
                    break;
                }

            }
        }


        private void DisplayBalance(Account account) => Console.WriteLine($"Your balance is now {account.Balance.RoundUp()}");

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

        private static bool ValidateMoneyInput(string input)
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
                    Console.WriteLine("Money cannot be 0 or negative number");
                    retVal = false;
                }
            }

            return retVal;
        }


    }

}
