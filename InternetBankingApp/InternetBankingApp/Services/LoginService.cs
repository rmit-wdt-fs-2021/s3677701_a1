using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InternetBankingApp.Services
{
    public class LoginService : ILoginService
    {
        public bool AuthenticateUser(string loginId)
        {
            return false;
        }

        public IList<Login> GetLogins()
        {
            using var client = new HttpClient();
            var loginJson = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").Result;
            var logins = JsonConvert.DeserializeObject<List<Login>>(loginJson);

            return logins;
        }
    }
}
