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
        List<Transaction> GetPagedTransactions(int accountNumber, int top, int? skip = 0);
        public List<Transaction> GetTransactions(int accountNumber);
        public Task InsertTransactionAsync(Transaction transaction);
    }
}
