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
        private readonly string _connectionString;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Account GetAccountByNumber(int accountNumber)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account";

            var accounts = command.GetDataTableAsync().Result.Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (string)x["AccountType"],
                CustomerID = (int)x["CustomerID"],
                Balance = (decimal)x["Balance"]
            }).ToList();

            return accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
        }

        public List<Account> GetAccounts(int customerID)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            return command.GetDataTableAsync().Result.Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (string)x["AccountType"],
                CustomerID = (int)x["CustomerID"],
                Balance = (decimal)x["Balance"]
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

        public async Task UpdateAccountBalanceAsync(Account account)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "update Account set Balance = @balance where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("balance", account.Balance);
            command.Parameters.AddWithValue("accountNumber", account.AccountNumber);

            await command.ExecuteNonQueryAsync();
        }
    }
}