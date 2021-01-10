using InternetBankingApp.Interfaces;
using InternetBankingApp.Managers;
using InternetBankingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InternetBankingApp.Services
{
    public class CustomerService
    {
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using var client = new HttpClient();
            var customerResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/")
                                               .ConfigureAwait(false);

            var customers = JsonConvert.DeserializeObject<List<Customer>>(customerResponse, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            });
            return customers;
        }

        public void InsertCustomers(string connectionString)
        {
            var customers = GetCustomersAsync().Result;

            var customerManager = new CustomerManagerProxy(connectionString);
            var accountManager = new AccountManager(connectionString);
            foreach (var customer in customers)
            {
                customerManager.InsertCustomerAsync(customer);

                foreach(var account in customer.Accounts)
                {
                    account.CustomerID = customer.CustomerID;
                    accountManager.InsertAccountAsync(account);
                }
            }
        }
    }
}
