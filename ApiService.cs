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
    public class ApiService
    {
        public async Task<Response> GetActivity(string urlBase, string controller)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Menssage = result,
                    };
                }

                var activity = JsonConvert.DeserializeObject<Activities>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = activity,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Menssage = ex.Message,
                };
            }
        }


    }
}
 