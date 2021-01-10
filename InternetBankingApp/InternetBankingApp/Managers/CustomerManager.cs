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

        public List<Customer> GetCustomers()
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";

            // TODO : var accountManager = new AccountManager(_connectionString);

            return command.GetDataTable().Select().Select(x => new Customer
            {
                CustomerID = (int)x["CustomerID"],
                Name = (string)x["Name"],
                Address = (string)x["Address"],
                City = (string)x["City"],
                PostCode = (string)x["PostCode"]
            }).ToList();
        }

        public void InsertCustomer(Customer customer)
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

            cmd.ExecuteNonQuery();
        }
    }

}
