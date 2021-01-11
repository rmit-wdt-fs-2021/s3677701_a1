using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ILoginService
    {
        public bool AuthenticateUser(string loginId, string password);
        public int GetCustomerIDFromLogin(string loginID);
    }
}
