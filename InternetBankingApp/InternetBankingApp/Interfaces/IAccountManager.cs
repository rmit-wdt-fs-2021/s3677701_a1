using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountManager
    {
        Account GetAccountByNumber(int accountNumber);
        public List<Account> GetAccounts(int customerID);
        public Task InsertAccountAsync(Account account);
        Task UpdateAccountBalanceAsync(Account account);
    }
}
