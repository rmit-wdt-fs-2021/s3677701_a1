using InternetBankingApp.Exceptions;
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
    public class AccountService : IAccountService
    {
        private readonly IAccountManager _accountManager;
        public AccountService(string connectionString)
        {
            _accountManager = new AccountManagerProxy(connectionString);
        }

        public Account GetAccount(string accountType, Customer customer)
        {
            return customer.Accounts.First(x => x.AccountType == accountType);
        }

        public Account GetAccountByNumber(int accountNumber)
        {
            return _accountManager.GetAccountByNumber(accountNumber);
        }

        public async Task AddBalanceAsync(Account account, decimal balance)
        {
            if (account is null)
            {
                throw new ArgumentNullException($"{nameof(account)} cannot be null.");
            }
            account.Balance += balance;
            await _accountManager.UpdateAccountBalanceAsync(account);
        }

        public async Task DeductBalanceAsync(Account account, decimal balance)
        {
            if (account is null)
            {
                throw new ArgumentNullException($"{nameof(account)} cannot be null.");
            }

            if(ValidateDeduction(account, balance))
            {
                // Update DB
                account.Balance -= balance;
                await _accountManager.UpdateAccountBalanceAsync(account);
            }
            else
            {
                throw new AccountBalanceUpdateException($"Account cannot be deducted by {nameof(balance)}: ${balance}.");
            }
        }

        public async Task InsertAccountsAsync(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                await _accountManager.InsertAccountAsync(account);
            }
        }

        private bool ValidateDeduction(Account account, decimal balance)
        {
            bool retVal = true;

            // Balance must not go below certain amounts as a result of withrawal.
            if (account.AccountType == "C")
            {
                if (account.Balance - balance <= 200)
                {
                    retVal = false;
                }
            }
            else if (account.AccountType == "S")
            {
                if (account.Balance - balance <= 0)
                {
                    retVal = false;
                }
            }

            return retVal;
        }
    }
}
