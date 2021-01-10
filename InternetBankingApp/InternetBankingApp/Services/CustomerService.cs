using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InternetBankingApp.Services
{
    public class CustomerService
    {
        public void GetCustomers()
        {
            using var client = new HttpClient();
            var customerResponse = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/");

        }
    }
}
