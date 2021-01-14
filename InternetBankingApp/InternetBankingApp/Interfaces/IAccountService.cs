using InternetBankingApp.Models;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Adds balance to an account.
        /// </summary>
        /// <param name="account">Balance for account to be updated.</param>
        /// <param name="balance">Amount to increase the balance by.</param>
        public Task AddBalanceAsync(Account account, decimal balance);

        /// <summary>
        /// Get an account of a particular type of a customer.
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="customer"></param>
        /// <returns>An account.</returns>
        public Account GetAccount(string accountType, Customer customer);

        /// <summary>
        /// Deducts account balance.
        /// </summary>
        /// <param name="account">Balance of account to be deducted.</param>
        /// <param name="balance">Balance for account to be updated.</param>
        public Task DeductBalanceAsync(Account account, decimal balance);

        /// <summary>
        /// Gets an account by its number.
        /// </summary>
        /// <param name="accountNumber">Unique number for identifying accounts.</param>
        /// <returns>An account.</returns>
        public Account GetAccountByNumber(int accountNumber);
    }
}
