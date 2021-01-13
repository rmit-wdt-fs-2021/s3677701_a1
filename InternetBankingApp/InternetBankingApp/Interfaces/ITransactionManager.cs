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
        public List<Transaction> GetTransactions(int accountNumber);
        public Task InsertTransactionAsync(Transaction transaction);
    }
}
