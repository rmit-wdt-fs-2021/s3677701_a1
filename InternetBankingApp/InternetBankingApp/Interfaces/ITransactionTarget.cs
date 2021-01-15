using InternetBankingApp.Models;
using System.Collections.Generic;

namespace InternetBankingApp.Interfaces
{
    public interface ITransactionTarget
    {
        /// <summary>
        /// Gets a list of paged transactions.
        /// </summary>
        /// <param name="accountNumber">Transactions for account.</param>
        /// <param name="top">Get this many entries.</param>
        /// <param name="skip">Skip this many entries</param>
        /// <returns>List of transactions</returns>
        public List<Transaction> GetPagedTransactions(int accountNumber, int top, int skip = 0);
    }
}
