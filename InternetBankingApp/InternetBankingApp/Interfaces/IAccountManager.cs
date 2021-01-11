using InternetBankingApp.Models;
using System.Collections.Generic;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountManager
    {
        public List<Account> GetAccounts(int customerID);
    }
}
