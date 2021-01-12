using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
