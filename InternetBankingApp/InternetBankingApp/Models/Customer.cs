using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
  
        public string Name { get; set; }

        // Property type is object as to cater for DBNull value.
        public object Address { get; set; }

        public object City { get; set; }

        public object PostCode { get; set; }

        public IList<Account> Accounts { get; set; }

        public bool HasSavingsAccount() => Accounts.Any(x => x.AccountType == "S");

        public bool HasCheckingAccount() => Accounts.Any(x => x.AccountType == "C");

    }
}
