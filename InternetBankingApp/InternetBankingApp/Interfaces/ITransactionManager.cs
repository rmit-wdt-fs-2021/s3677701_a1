using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ITransactionManager
    {
        List<Transaction> GetTransactions(int accountNumber);
        Task InsertTransactionAsync(Transaction transaction);
    }
}
