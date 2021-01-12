using InternetBankingApp.Models;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountService
    {
        Task AddBalanceAsync(Account account, decimal balance);
        Account GetAccount(string accountType, Customer customer);
    }
}
