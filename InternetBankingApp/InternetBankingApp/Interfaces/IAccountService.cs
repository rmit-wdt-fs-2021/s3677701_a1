using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountService
    {
        public Task AddBalanceAsync(Account account, decimal balance);
        public Account GetAccount(string accountType, Customer customer);
        public Task DeductBalanceAsync(Account account, decimal balance);
    }
}
