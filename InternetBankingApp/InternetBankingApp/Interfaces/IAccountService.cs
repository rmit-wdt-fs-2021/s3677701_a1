using InternetBankingApp.Models;

namespace InternetBankingApp.Interfaces
{
    public interface IAccountService
    {
        Account GetAccount(string accountType, Customer customer);
    }
}
