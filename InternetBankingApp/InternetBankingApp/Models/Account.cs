using System.Collections.Generic;
using System.Linq;
using System;

namespace InternetBankingApp.Models
{

    public class Account
    {
        public int AccountNumber { get; set; }

        public string AccountType { get; set; }

        public int CustomerID { get; set; }

        public decimal Balance { get; set; }

        public IList<Transaction> Transactions { get; set; }

        private const int FreeTransactions = 4;

        public int ChargeableTransactions { get; set; } = 0;

        public bool HasFreeTransaction() => Transactions.Count(x => x.TransactionType == "T" || x.TransactionType == "W") <= FreeTransactions;
    }
}
