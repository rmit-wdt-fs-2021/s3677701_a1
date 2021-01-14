using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountManager
    {
        /// <summary>
        /// Gets an account by its unique number.
        /// </summary>
        /// <param name="accountNumber">An unique integer for identifying accounts.</param>
        /// <returns>Account</returns>
        Account GetAccountByNumber(int accountNumber);

        /// <summary>
        /// Gets all accounts for a particular customer.
        /// </summary>
        /// <param name="customerID">An unique integer for identifying customers.</param>
        /// <returns>A list of accounts.</returns>
        public List<Account> GetAccounts(int customerID);

        /// <summary>
        /// Inserts a new account in the Account table of the database.
        /// </summary>
        /// <param name="account">Account to be inserted.</param>
        public Task InsertAccountAsync(Account account);

        /// <summary>
        /// Updates the balance of an account in the database.
        /// </summary>
        /// <param name="account">Balance of the account to be updated.</param>
        /// <returns></returns>
        public Task UpdateAccountBalanceAsync(Account account);
    }
}
