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

        public async Task AddBalanceAsync(Account account, decimal balance)
        {
            if (account is null)
            {
                throw new ArgumentNullException("Account cannot be null");
            }
            account.Balance += balance;
            await _accountManager.UpdateAccountBalanceAsync(account);
        }

    }
}
