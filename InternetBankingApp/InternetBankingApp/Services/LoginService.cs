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

namespace InternetBankingApp.Services
{
    public class LoginService
    {
        public bool AuthenticateUser(string loginId)
        {
            // Call to db ?
            return false;
        }

        public async Task<IList<Login>> GetLoginsAsync()
        {
            using var client = new HttpClient();
            var loginResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").ConfigureAwait(false);
            var logins = JsonConvert.DeserializeObject<List<Login>>(loginResponse);

            return logins;
        }

        public async Task InsertLoginsAsync(string connectionString)
        {
            var logins = await GetLoginsAsync().ConfigureAwait(false);
            var loginManager = new LoginManager(connectionString);
            foreach(var login in logins)
            {
                await loginManager.InsertLoginAsync(login).ConfigureAwait(false);
            }
        }
    }
}
