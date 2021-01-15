using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(string transactionType, Account account, decimal amount, DateTime transactionTime,
                                 int? destinationAccountNumber = null, string comment = null);
        List<Transaction> GetPagedTransactions(Account account, int top = 4, int skip = 0);
    }
}
