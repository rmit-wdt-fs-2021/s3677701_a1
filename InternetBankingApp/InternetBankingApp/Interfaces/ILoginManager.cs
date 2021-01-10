using InternetBankingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBankingApp.Interfaces
{
    public interface ILoginManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public List<Login> GetLogin(string loginID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task InsertLoginAsync(Login login);
    }
}
