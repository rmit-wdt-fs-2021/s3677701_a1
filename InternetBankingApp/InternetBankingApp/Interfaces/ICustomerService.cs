using InternetBankingApp.Models;

namespace InternetBankingApp.Interfaces
{
    public interface ICustomerService
    {
        public Customer GetCustomer(int customerID);
    }
}
