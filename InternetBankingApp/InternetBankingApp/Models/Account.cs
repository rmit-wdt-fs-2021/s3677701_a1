using System.Collections.Generic;
using System.Linq;

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

        public bool HasFreeTransaction => Transactions.Where(x => x.TransactionType == "W")
                                                      .Where(x => x.TransactionType == "T")
                                                      .Count() <= FreeTransactions;
    }
}
