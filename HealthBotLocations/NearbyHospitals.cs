using HealthBotLocations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using HealthBotLocations.Helpers;

/// <summary>
/// Uses the Bing REST Api to find the hospitals closest to the input Lat/Long via the route parameters.  
/// </summary>
namespace HealthBotLocations
{
    public static class NearbyHospitals
    {
        [FunctionName("NearbyHospitals")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "nearby/{latitude}/{longitude}")] HttpRequest req,
            double latitude,
            double longitude,
            ILogger log)
        {
            List<Location> locationResults = new List<Location>();
            string BingMapsAPIKey = Environment.GetEnvironmentVariable("BingMapKey");
            string point = $"{latitude},{longitude}";

            // Each Entity Type should be separated by comma ',' 
            // Type Ids are here: https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/
            string entityTypes = "Hospitals";

            string bingEntitySearchTemaplate = $"https://dev.virtualearth.net/REST/v1/LocalSearch/?type={entityTypes}&userLocation={point}&key={BingMapsAPIKey}";

            // Image With Route
            string wp0 = $"{latitude},{longitude}";
            string wp1 = "{0},{1}";
            string bingMapTemplate = "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/Routes/?wp.0={0};26;1&wp.1={1};28;2&key={2}";
            string bingRouteTemplate = "http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={0}&wp.1={1}&du=mi&key={2}";

            try
            {
                HttpClient client = new HttpClient();
                string results = await client.GetStringAsync(bingEntitySearchTemaplate);
                BingResponse bingresults = JsonConvert.DeserializeObject<BingResponse>(results);

                if (bingresults.StatusCode == 200)
                {
                    ResourceSet bingLocationSet = bingresults.ResourceSets.FirstOrDefault();
                    List<Resource> bingLocations = bingLocationSet.Resources.Take(3).ToList();

                    Location l;
                    // Get the Map for the top 3 items
                    foreach (Resource r in bingLocations)
                    {
                        l = new Location
                        {
                            Name = r.Name,
                            Telephone = r.PhoneNumber,
                            Address = r.Address.FormattedAddress,
                            Point = r.Point,
                            Url = r.WebSite
                        };

                        #region Get Map Image
                        //TODO: Ensure to deal with failures/errors.
                        string requestedMapUri = string.Format(bingMapTemplate, wp0, string.Format(wp1, l.Point.Coordinates[0], l.Point.Coordinates[1]), BingMapsAPIKey);

                        var map = await client.GetStreamAsync(requestedMapUri);

                        //Upload file to the container
                        AzureStorageConfig asc = new AzureStorageConfig
                        {
                            AccountKey = Environment.GetEnvironmentVariable("BlobAccountKey"),
                            AccountName = Environment.GetEnvironmentVariable("BlobAccountName"),
                            ImageContainer = Environment.GetEnvironmentVariable("BlobImageContainer")
                        };

                        string fileName = Guid.NewGuid().ToString() + ".png";

                        string blobUrl = await StorageHelper.UploadFileToStorage(map, fileName, asc);

                        //Write to the blob and get URI                        
                        l.MapUri = blobUrl;
                        #endregion

                        #region Get Route Details
                        string requestedRouteUri = string.Format(bingRouteTemplate, wp0, string.Format(wp1, l.Point.Coordinates[0], l.Point.Coordinates[1]), BingMapsAPIKey);

                        string routeData = await client.GetStringAsync(requestedRouteUri);

                        BingResponse bingRouteResults = JsonConvert.DeserializeObject<BingResponse>(routeData);
                        ResourceSet routeResourceSet = bingRouteResults.ResourceSets.FirstOrDefault();

                        if (routeResourceSet != null)
                        {
                            Resource routeResource = routeResourceSet.Resources.FirstOrDefault();
                            if (routeResource != null)
                            {
                                l.Distance = $"{routeResource.TravelDistance:0.0} mi.";
                            }
                        }
                        #endregion

                        locationResults.Add(l);
                    }
                }
                else
                {
                    return new BadRequestResult();
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
