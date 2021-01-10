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
        public List<Customer> GetCustomers()
        {
            using var client = new HttpClient();
            var customerResponse = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;

            var customers = JsonConvert.DeserializeObject<List<Customer>>(customerResponse, new JsonSerializerSettings
            {
                // See here for DateTime format string documentation:
                // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                DateFormatString = "dd/MM/yyyy"
            });
            Console.Write(customers);
            return customers;
        }

        public void InsertCustomers(string connectionString)
        {
            var customers = GetCustomers();
            var customerManager = new CustomerManagerProxy(connectionString);
            foreach (var customer in customers)
            {
                customerManager.InsertCustomer(customer);
            }
        }
    }
}
