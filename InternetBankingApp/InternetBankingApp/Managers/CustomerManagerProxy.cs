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
    public class CustomerManagerProxy : ICustomerManager
    {
        private readonly string _connectionString;

        private List<Customer> _customers;

        public CustomerManagerProxy(string connectionString)
        {
            _connectionString = connectionString;

        }

        public List<Customer> GetCustomers()
        {
            if (_customers.Any())
            {
                return _customers;
            }
            else
            {
                using var connection = _connectionString.CreateConnection();
                var command = connection.CreateCommand();
                command.CommandText = "select * from Customer";

                // TODO : var accountManager = new AccountManager(_connectionString);

                _customers = command.GetDataTable().Select().Select(x => new Customer
                {
                    CustomerID = (int)x["CustomerID"],
                    Name = (string)x["Name"],
                    Address = (string)x["Address"],
                    City = (string)x["City"],
                    PostCode = (string)x["PostCode"]
                }).ToList();
                return _customers;
            }
        }

        public async Task InsertCustomerAsync(Customer customer)
        {
            if (_customers.Any())
            {
                return;
            }

            using var connection = _connectionString.CreateConnection();
            connection.Open();
            // TODO : add try/catch ?
            var cmd = connection.CreateCommand();
            cmd.CommandText = "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
            cmd.Parameters.AddWithValue("customerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("address", customer.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("city", customer.City ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("postCode", customer.PostCode ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
