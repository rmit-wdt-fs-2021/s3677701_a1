using InternetBankingApp.Interfaces;
using InternetBankingApp.Managers;
using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;
        private readonly ITransactionManager _transactionManager;
        private readonly IAccountService _accountService;

        public TransactionService(string connectionString)
        {
            _transactionManager = new TransactionManagerProxy(connectionString);
            _accountService = new AccountService(connectionString);
        }

        public async Task AddTransactionAsync(string transactionType, int accountNumber, decimal amount,
                                              DateTime transactionTime, int? destinationAccountNumber = null,
                                              string comment = null)
        {
            if (transactionType is null || accountNumber == destinationAccountNumber || transactionType is null)
            {
                throw new ArgumentException();
            }

            if (accountNumber.ToString().Length != 4 || destinationAccountNumber?.ToString().Length != 4)
            {
                throw new ArgumentException($"{nameof(accountNumber)} and {nameof(destinationAccountNumber)} must be 4 digits.");
            }

            if (transactionType == "T")
            {
                if (destinationAccountNumber == null)
                {
                    throw new ArgumentNullException($"{nameof(destinationAccountNumber)} cannot be null when transfering money");
                }

                // Update account balances
                await UpdateTransferAccountBalances(accountNumber, amount, destinationAccountNumber.Value);
            }

            var transaction = CreateTransaction(transactionType, accountNumber, amount, transactionTime, destinationAccountNumber, comment);
            await _transactionManager.InsertTransactionAsync(transaction);
        }

        private async Task UpdateTransferAccountBalances(int accountNumber, decimal amount, int destinationAccountNumber)
        {
            var srcAccount = _accountService.GetAccountByNumber(accountNumber);
            var destAccount = _accountService.GetAccountByNumber(destinationAccountNumber);
            await _accountService.DeductBalanceAsync(srcAccount, amount);
            await _accountService.AddBalanceAsync(destAccount, amount);
        }

        private static Transaction CreateTransaction(string transactionType, int accountNumber, decimal amount,
                                                     DateTime transactionTime, int? destinationAccountNumber = null,
                                                     string comment = null)
        {
            return new Transaction
            {
                TransactionType = transactionType,
                AccountNumber = accountNumber,
                Amount = amount,
                TransactionTimeUtc = transactionTime,
                DestinationAccountNumber = destinationAccountNumber,
                Comment = comment
            };
        }
    }
}
