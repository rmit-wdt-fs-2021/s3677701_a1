using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ICustomerManager
    {
        public List<Customer> GetAllCustomers();

        public Task InsertCustomerAsync(Customer customer);
        public Customer GetCustomer(int customerID);
    }
}
