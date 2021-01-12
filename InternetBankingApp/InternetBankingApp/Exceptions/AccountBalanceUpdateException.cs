using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Exceptions
{
    public class AccountBalanceUpdateException : Exception
    {
        public AccountBalanceUpdateException(string message) : base(message)
        {

        }
    }
}
