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
            _customers = new List<Customer>();
        }

        public Customer GetCustomer(int customerID)
        {
            if (_customers.Any())
            {
                return _customers.FirstOrDefault(x => x.CustomerID == customerID);
            }

            return GetAllCustomers().FirstOrDefault(x => x.CustomerID == customerID);
        }

        public List<Customer> GetAllCustomers()
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

                var accountManager = new AccountManager(_connectionString); 
              
                    _customers = command.GetDataTable().Select().Select(x => new Customer
                    {
                        CustomerID = (int)x["CustomerID"],
                        Name = (string)x["Name"],
                        Address = x["Address"],
                        City = x["City"],
                        PostCode = x["PostCode"],
                        Accounts = accountManager.GetAccounts((int)x["CustomerID"])
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
