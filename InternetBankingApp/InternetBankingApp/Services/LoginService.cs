using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InternetBankingApp.Managers;
using SimpleHashing;

namespace InternetBankingApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginManager _loginManager;

        public LoginService(string connectionString)
        {
            _loginManager = new LoginManagerProxy(connectionString);
        }

        public bool AuthenticateUser(string loginId, string password)
        {
            var login = _loginManager.GetLogin(loginId);
            return login != null && PBKDF2.Verify(login.PasswordHash, password);
        }

        private async Task<IList<Login>> GetLoginsAsync()
        {
            using var client = new HttpClient();
            var loginResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");
            var logins = JsonConvert.DeserializeObject<List<Login>>(loginResponse);

            return logins;
        }

        public void InsertLogins()
        {
            var logins = GetLoginsAsync().Result;
            foreach (var login in logins)
            {
                _loginManager.InsertLoginAsync(login);
            }
        }
    }
}
