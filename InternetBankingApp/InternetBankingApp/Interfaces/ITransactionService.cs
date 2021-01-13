using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(string transactionType, int accountNumber, decimal amount, DateTime transactionTime, int? destinationAccountNumber = null, string comment = null);
        List<Transaction> GetPagedTransactions(Account account, int? top = null);
    }
}
