using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace InternetBankingApp.Managers
{
    public class AccountManager : IAccountManager
    {
        private string _connectionString;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Account> GetAccount(int customerID)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            return command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (string)x["AccountType"],
                CustomerID = (int)x["CustomerID"],
                Balance = (double)x["Balance"]
            }).ToList();
        }


    }
}