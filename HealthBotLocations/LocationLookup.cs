using HealthBotLocations.Helpers;
using HealthBotLocations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Looks up the closest location from a list of locations in a Cosmos DB using Cosmos Geospatial query.
/// </summary>
namespace HealthBotLocations
{
    public static class LocationLookup
    {
        [FunctionName("LocationLookup")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "custom/{latitude}/{longitude}")] HttpRequest req,
            [CosmosDB(Constants.CosmosDbName,
                      Constants.MyLocationsCollection, 
                      CreateIfNotExists = true,
                      ConnectionStringSetting = "AzureCosmosDBConnectionString",
                      SqlQuery = "SELECT FROM locations c WHERE ST_DISTANCE < 10000"
            )] IEnumerable<Location> locations,
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

                BingHelper bh = new BingHelper();

                string mapUrl;
                string distance;

                foreach (Location l in locations)
                {
                    WayPoint wp = new WayPoint
                    {
                        Latitude = l.Point.Coordinates[0],
                        Longitude = l.Point.Coordinates[1]
                    };

                    mapUrl = await bh.GetMapImageUrl(userLocation, wp);
                    distance = await bh.GetRouteDistance(userLocation, wp);

                    l.MapUri = mapUrl;
                    l.Distance = distance;

                    locationResults.Add(l);
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            return new OkObjectResult(locationResults);
        }
    }
}
