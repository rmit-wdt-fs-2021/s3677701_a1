using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly string _connectionString;

        public TransactionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Transaction> GetPagedTransactions(int accountNumber, int top, int? skip = 0)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetTransactions(int accountNumber)
        {
            throw new NotImplementedException();
        }

        public async Task InsertTransactionAsync(Transaction transaction)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
                "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUtc)";
            command.Parameters.AddWithValue("transactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("AccountNumber", transaction.AccountNumber);
            command.Parameters.AddWithValue("DestinationAccountNumber", transaction.DestinationAccountNumber);
            command.Parameters.AddWithValue("Amount", transaction.Amount);
            command.Parameters.AddWithValue("Comment", transaction.Comment);
            command.Parameters.AddWithValue("TransactionTimeUtc", transaction.TransactionTimeUtc);

            await command.ExecuteNonQueryAsync();
        }
    }
}
