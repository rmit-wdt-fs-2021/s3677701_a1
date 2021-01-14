using InternetBankingApp.Models;
using System.Collections.Generic;

namespace InternetBankingApp.Interfaces
{
    public interface ITransactionTarget
    {
        public List<Transaction> GetPagedTransactions(int accountNumber, int top, int skip = 0);
    }
}
