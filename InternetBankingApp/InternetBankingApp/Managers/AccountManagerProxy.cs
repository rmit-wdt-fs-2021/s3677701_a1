using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Managers
{
    public class AccountManagerProxy : IAccountManager
    {
        private readonly string _connectionString;
        
        public List<Account> Accounts { get; }
        public AccountManagerProxy(string connectionString)
        {
            _connectionString = connectionString;

            using var connection = _connectionString.CreateConnection();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from Account";
            Accounts = command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (string)x["AccountType"], //TODO make enum?
                CustomerID = (int)x["CustomerID"],
                Balance = (decimal)x["Balance"]
                //Transactions
            }).ToList();

        }
        public List<Account> GetAccounts(int customerID)
        {
            return Accounts.Where(x => x.CustomerID == customerID).ToList();
        }

        public async Task InsertAccountAsync(Account account)
        {
            if (Accounts.Any())
            {
                return;
            }

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
