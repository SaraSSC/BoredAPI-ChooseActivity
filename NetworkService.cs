using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Net;

namespace SecondTryTest
{
    public class NetworkService
    {
        public Response CheckConnection()
        {
            var client = new WebClient();

            try
            {
                using (client.OpenRead("http://www.google.com"))
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Menssage = "Connection is OK"
                    };
                }
            }
            catch (WebException ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Menssage = ex.Message
                };
            }
        }
    }
}
