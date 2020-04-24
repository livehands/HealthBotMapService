using HealthBotLocations.Helpers;
using HealthBotLocations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBotLocations
{
    public static class NearbyHospitalsBing
    {
        [FunctionName("NearbyHospitalsBing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bing/{latitude}/{longitude}")] HttpRequest req,
            double latitude,
            double longitude,
            ILogger log)
        {
            List<Location> locationResults = new List<Location>();

            try
            {
                WayPoint userLocation = new WayPoint
                {
                    Longitude = longitude,
                    Latitude = latitude
                };

                locationResults = (await NearbyHelper.GetNearbyLocations(new BingHelper(), userLocation)).ToList();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            return new OkObjectResult(locationResults);
        }
    }
}
