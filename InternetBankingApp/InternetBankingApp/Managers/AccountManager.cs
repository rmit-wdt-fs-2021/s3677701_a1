using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task InsertAccountAsync(Account account)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = 
                "insert into Account (AccountNumber, AccountType, CustomerID, Balance) values (@accountNumber, @accountType, @customerID, @balance)";
            command.Parameters.AddWithValue("accountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("accountType", account.AccountType);
            command.Parameters.AddWithValue("customerID", account.CustomerID);
            command.Parameters.AddWithValue("Balance", account.Balance);

            await command.ExecuteNonQueryAsync();
        }
    }
}