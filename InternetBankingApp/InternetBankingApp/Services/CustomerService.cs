using InternetBankingApp.Interfaces;
using InternetBankingApp.Managers;
using InternetBankingApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
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

        public CustomerService(string connectionString)
        {
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
            List<Customer> customers;
            using var client = new HttpClient();
            try
            {
                var customerResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");
                customers = JsonConvert.DeserializeObject<List<Customer>>(customerResponse, new JsonSerializerSettings
                {
                    DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
                });
                if (IsValidData(customers))
                {
                    throw new JsonException("Error reading customer data.");
                }
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Unable to contact web service. Please try again later.");
                Console.WriteLine(e.Message);
                throw;
            }
          
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

        private bool IsValidData(List<Customer> customers)
        {
            bool retVal = true;
            foreach(var customer in customers)
            {
                if(customer.CustomerID.ToString().Length != 4)
                {
                    retVal = false;
                }

                foreach(var account in customer.Accounts)
                {
                    if(account.AccountNumber.ToString().Length != 4)
                    {
                        retVal = false;
                    }

                    if(account.AccountType != "S" || account.AccountType != "C")
                    {
                        retVal = false;
                    }

                    if (customer.HasCheckingAccount())
                    {
                        if(customer.CheckingAccount.Balance < 200)
                        {
                            retVal = false;
                        }
                    }

                    if (customer.HasSavingsAccount())
                    {
                        if(customer.SavingsAccount.Balance <= 0)
                        {
                            retVal = false;
                        }
                    }
                }
            }

            return retVal;
        }
    }
}
