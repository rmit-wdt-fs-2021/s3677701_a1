﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        
        public string Name { get; set; }

        public string Address { get; set; }

        public IList<Account> Accounts { get; set; }

    }
}
