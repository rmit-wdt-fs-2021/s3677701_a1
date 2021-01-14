﻿using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace InternetBankingApp.Managers
{
    public class TransactionManagerAdapter : TransactionManagerProxy, ITransactionTarget
    {
        public TransactionManagerAdapter(string connectionString) : base(connectionString)
        {

        }

        public List<Transaction> GetPagedTransactions(int accountNumber, int top, int skip = 0)
        {
            return GetTransactions(accountNumber).Skip(skip).Take(top)
                                                 .OrderByDescending(x => x.TransactionTimeUtc).ToList();
        }

    }
}
