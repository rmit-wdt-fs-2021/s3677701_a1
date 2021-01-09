using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Models
{
    public enum AccountType
    {
        Savings,
        Checking
    }

    public class Account
    {
        public int AccountNumber { get; set; }

        public string AccountType { get; set; }
        
        public string CustomerID { get; set; }

        public double Balance { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
