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
            _connectionString = connectionString;
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

            if (accountNumber.ToString().Length != 4)
            {
                throw new ArgumentException($"{nameof(accountNumber)} must be 4 digits.");
            }

            switch (transactionType)
            {
                case "T":
                    if (destinationAccountNumber == null)
                    {
                        throw new ArgumentNullException($"{nameof(destinationAccountNumber)} cannot be null when transfering money");
                    }
                    else if (destinationAccountNumber.ToString().Length != 4)
                    {
                        throw new ArgumentException($"{nameof(destinationAccountNumber)} must be 4 digits.");
                    }

                    await UpdateTransferAccountBalances(accountNumber, amount, destinationAccountNumber.Value);
                    break;
                case "D":
                    await UpdateDepositAccountBalance(accountNumber, amount);
                    break;
                case "W":
                    await UpdateWithdrawnAccountBalance(accountNumber, amount);
                    break;
                case "S":
                    var account = _accountService.GetAccountByNumber(accountNumber);
                    if (account.HasFreeTransaction())
                    {
                        return;
                    }

                    await ApplyServiceCharge(account, amount);
                    break;
                default:
                    throw new ArgumentException($"Unknown transaction type : {transactionType}");
            }

            var transaction = CreateTransaction(transactionType, accountNumber, amount, transactionTime, destinationAccountNumber, comment);
            await _transactionManager.InsertTransactionAsync(transaction);
        }

        private async Task ApplyServiceCharge(Account accountToCharge, decimal serviceCharge)
        {
            await _accountService.DeductBalanceAsync(accountToCharge, serviceCharge);
        }

        private async Task UpdateWithdrawnAccountBalance(int accountNumber, decimal amount)
        {
            var account = _accountService.GetAccountByNumber(accountNumber);
            await _accountService.DeductBalanceAsync(account, amount);
        }

        private async Task UpdateDepositAccountBalance(int accountNumber, decimal amount)
        {
            await _accountService.AddBalanceAsync(_accountService.GetAccountByNumber(accountNumber), amount);
        }

        private async Task UpdateTransferAccountBalances(int accountNumber, decimal amount, int destinationAccountNumber)
        {
            var srcAccount = _accountService.GetAccountByNumber(accountNumber);
            var destAccount = _accountService.GetAccountByNumber(destinationAccountNumber);
            await _accountService.DeductBalanceAsync(srcAccount, amount);
            await _accountService.AddBalanceAsync(destAccount, amount);
        }

        public List<Transaction> GetPagedTransactions(Account account, int top = 4, int skip = 0)
        {
            ITransactionTarget transactionAdapter = new TransactionManagerAdapter(_connectionString);
            return transactionAdapter.GetPagedTransactions(account.AccountNumber, top, skip);
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
