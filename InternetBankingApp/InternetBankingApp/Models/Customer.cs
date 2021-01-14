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

        public string Address { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public IList<Account> Accounts { get; set; }

        public Account SavingsAccount => Accounts.FirstOrDefault(x => x.AccountType == "S");

        public Account CheckingAccount => Accounts.FirstOrDefault(x => x.AccountType == "C");

        public bool HasSavingsAccount() => Accounts.Any(x => x.AccountType == "S");

        public bool HasCheckingAccount() => Accounts.Any(x => x.AccountType == "C");

    }
}
