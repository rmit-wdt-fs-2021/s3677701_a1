using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Services
{
    public class AccountService : IAccountService
    {
        public Account GetAccount(string accountType, Customer customer)
        {
            return customer.Accounts.First(x => x.AccountType == accountType);
        }

    }
}
