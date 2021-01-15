using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly string _connectionString;
        public CustomerManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Customer GetCustomer(int customerID)
        {
            return GetAllCustomers().FirstOrDefault(x => x.CustomerID == customerID);

        }

        public List<Customer> GetAllCustomers()
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";

            var accountManager = new AccountManager(_connectionString);

            return command.GetDataTableAsync().Result.Select().Select(x => new Customer
            {
                CustomerID = (int)x["CustomerID"],
                Name = (string)x["Name"],
                Address = Convert.IsDBNull(x["Address"]) ? null : (string)x["Address"],
                City = Convert.IsDBNull(x["City"]) ? null : (string)x["City"],
                PostCode = Convert.IsDBNull(x["PostCode"]) ? null : (string)x["PostCode"],
                Accounts = accountManager.GetAccounts((int)x["CustomerID"])
            }).ToList();
        }

        public async Task InsertCustomerAsync(Customer customer)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
            cmd.Parameters.AddWithValue("customerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("address", customer.Address);
            cmd.Parameters.AddWithValue("city", customer.City);
            cmd.Parameters.AddWithValue("postCode", customer.PostCode);

            await cmd.ExecuteNonQueryAsync();
        }
    }

}
