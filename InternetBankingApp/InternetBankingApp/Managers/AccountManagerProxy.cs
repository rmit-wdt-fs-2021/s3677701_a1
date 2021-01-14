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
        
        public List<Account> Accounts { get; set; }

        public AccountManagerProxy(string connectionString)
        {
            _connectionString = connectionString;
            Accounts = GetAllAccouts();
        }

        private List<Account> GetAllAccouts()
        {
            using var connection = _connectionString.CreateConnection();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from Account";

            ITransactionManager transactionManager = new TransactionManagerProxy(_connectionString);
            return command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (string)x["AccountType"],
                CustomerID = (int)x["CustomerID"],
                Balance = (decimal)x["Balance"],
                Transactions = transactionManager.GetTransactions((int)x["AccountNumber"])
            }).ToList();
        }

        public List<Account> GetAccounts(int customerID)
        {
            return Accounts.Where(x => x.CustomerID == customerID).ToList();
        }

        public Account GetAccountByNumber(int accountNumber)
        {
            return Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
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

        public async Task UpdateAccountBalanceAsync(Account account)
        {
            if (account is null)
            {
                throw new ArgumentNullException($"{nameof(account)} cannot be null");
            }

            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "update Account set Balance = @balance where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("balance", account.Balance);
            command.Parameters.AddWithValue("accountNumber", account.AccountNumber);

            await command.ExecuteNonQueryAsync();
            Accounts = GetAllAccouts();
            // TODO : err handle if update not successful
        }
    }
}
