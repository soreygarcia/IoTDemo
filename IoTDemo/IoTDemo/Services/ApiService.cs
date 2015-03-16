using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IoTDemo.Services
{
    public class ApiService
    {
        const string securityKey = "eucDjzXuAqWeOmVDvKBCqOFFlNIeZB88";
        public async Task<List<IoTDemo.Models.EventLog>> GetEvents()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", securityKey);
                HttpResponseMessage result = await client.GetAsync(new Uri("https://cloudrsmed.azure-mobile.net/tables/EventLog?__systemProperties=createdAt"));
                string json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<IoTDemo.Models.EventLog>>(json);
            }
        }
    }
}
