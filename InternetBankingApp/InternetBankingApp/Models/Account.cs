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

        //public bool HasFreeTransaction()
        //{
        //    if(Transactions == null)
        //    {
        //        Console.WriteLine("No transaction for " + AccountNumber);
        //        return false;
        //    }
        //    return Transactions
        //      .Count(x => x.TransactionType == "T" || x.TransactionType == "W") <= FreeTransactions;
        //}
    }
}
