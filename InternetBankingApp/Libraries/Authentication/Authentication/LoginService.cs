using Authentication.Interfaces;
using Authentication.Manager;
using Authentication.Model;
using Newtonsoft.Json;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authentication
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

        public int GetCustomerIDFromLogin(string loginID)
        {
            var login = _loginManager.GetLogin(loginID);
            return login.CustomerID;
        }

        private async Task<IList<Login>> GetLoginsAsync()
        {
            List<Login> logins;
            using var client = new HttpClient();
            try
            {
                var loginResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");
                logins = JsonConvert.DeserializeObject<List<Login>>(loginResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to contact login web service. Please try again later.");
                Console.WriteLine(e.Message);
                throw;
            }
            return logins;
        }

        public async Task InsertLoginsAsync()
        {
            var logins = await GetLoginsAsync();
            foreach (var login in logins)
            {
                await _loginManager.InsertLoginAsync(login);
            }
        }
    }
}
