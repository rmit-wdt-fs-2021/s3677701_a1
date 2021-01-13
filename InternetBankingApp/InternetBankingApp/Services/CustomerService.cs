using InternetBankingApp.Interfaces;
using InternetBankingApp.Managers;
using InternetBankingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InternetBankingApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerManager _customerManager;
        private readonly IAccountManager _accountManager;
        private readonly ITransactionManager _transactionManager;
        private readonly string _connectionString;

        public CustomerService(string connectionString)
        {
            _connectionString = connectionString;
            _customerManager = new CustomerManagerProxy(connectionString);
            _accountManager = new AccountManagerProxy(connectionString);
            _transactionManager = new TransactionManagerProxy(connectionString);
        }

        public Customer GetCustomer(int customerID)
        {
            return _customerManager.GetCustomer(customerID);
        }


        private async Task<List<Customer>> GetCustomersAsync()
        {
            using var client = new HttpClient();
            var customerResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");

            var customers = JsonConvert.DeserializeObject<List<Customer>>(customerResponse, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
            });
            return customers;
        }

        public async Task InsertCustomerDataAsync()
        {
            if (_customerManager.GetAllCustomers().Any())
            {
                return;
            }
            var customers = await GetCustomersAsync();
            foreach (var customer in customers)
            {
                await _customerManager.InsertCustomerAsync(customer);

                foreach(var account in customer.Accounts)
                {
                    account.CustomerID = customer.CustomerID;
                    await _accountManager.InsertAccountAsync(account);

                    foreach(var transaction in account.Transactions)
                    {
                        transaction.AccountNumber = account.AccountNumber;
                        transaction.Amount = account.Balance;
                        transaction.TransactionType = "D";
                        await _transactionManager.InsertTransactionAsync(transaction);
                    }
                }
            }
        }
    }
}
