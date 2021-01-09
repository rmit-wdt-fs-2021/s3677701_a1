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
        public IList<Login> GetLogins();
        public bool AuthenticateUser(string loginId);
    }
}
