﻿using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Managers
{
    public class TransactionManagerProxy : ITransactionManager
    {
        private readonly string _connectionString;

        public List<Transaction> Transactions { get; }

        public TransactionManagerProxy(string connectionString)
        {
            _connectionString = connectionString;

            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction]";

            Transactions = command.GetDataTable().Select().Select(x => new Transaction
            {
                TransactionID = (int)x["TransactionID"],
                TransactionType = (string)x["TransactionType"],
                AccountNumber = (int)x["AccountNumber"],
                DestinationAccountNumber = Convert.IsDBNull(x["DestinationAccountNumber"]) ? null : (int?)x["DestinationAccountNumber"],
                Amount = (decimal)x["Amount"],
                Comment = Convert.IsDBNull(x["Comment"]) ? null : (string)x["Comment"],
                TransactionTimeUtc = (DateTime)x["TransactionTimeUtc"]
            }).ToList();
        }

        public List<Transaction> GetTransactions(int accountNumber)
        {
            return Transactions.Where(x => x.AccountNumber == accountNumber).ToList();
        }

        public List<Transaction> GetPagedTransactions(int accountNumber, int top, int? skip = 0)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction]";

            return command.GetDataTable().Select().Select(x => new Transaction
            {
                TransactionID = (int)x["TransactionID"],
                TransactionType = (string)x["TransactionType"],
                AccountNumber = (int)x["AccountNumber"],
                DestinationAccountNumber = Convert.IsDBNull(x["DestinationAccountNumber"]) ? null : (int?)x["DestinationAccountNumber"],
                Amount = (decimal)x["Amount"],
                Comment = Convert.IsDBNull(x["Comment"]) ? null : (string)x["Comment"],
                TransactionTimeUtc = (DateTime)x["TransactionTimeUtc"]
            }).Where(x => x.AccountNumber == accountNumber)
            .AsEnumerable()
            .Skip(skip.Value)
            .Take(top)
            .ToList();
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
            command.Parameters.AddWithValue("DestinationAccountNumber", transaction.DestinationAccountNumber is null ? DBNull.Value : transaction.DestinationAccountNumber);
            command.Parameters.AddWithValue("Amount", transaction.Amount);
            command.Parameters.AddWithValue("Comment", transaction.Comment is null ? DBNull.Value : transaction.Comment);
            command.Parameters.AddWithValue("TransactionTimeUtc", transaction.TransactionTimeUtc);

            await command.ExecuteNonQueryAsync();
        }
    }
}
