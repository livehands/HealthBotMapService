using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthBotLocations
{
    public static class LocationViaIP
    {
        [FunctionName("LocationViaIP")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "locationip")] HttpRequest req,
            ILogger log)
        {
            string IPServiceEndpoint = Environment.GetEnvironmentVariable("IPLocationUri"); //"https://geolocation-db.com/jsonp";

            HttpClient client = new HttpClient();
            
            string clientIP = req.HttpContext.Connection.RemoteIpAddress.ToString();
            var data = await client.GetStringAsync(IPServiceEndpoint + clientIP);

            return new OkObjectResult(data);
        }
    }
}
