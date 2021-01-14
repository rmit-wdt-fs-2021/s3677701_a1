using Authentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interfaces
{
    public interface ILoginManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public Login GetLogin(string loginID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task InsertLoginAsync(Login login);
    }
}
