using HealthBotLocations.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthBotLocations.Helpers
{
    public class GoogleHelper : ILocationApi
    {
        private string MapsAPIKey = Environment.GetEnvironmentVariable("GoogleMapKey");
        private HttpClient client;

        public GoogleHelper()
        {
            client = new HttpClient();
        }

        public async Task<IEnumerable<Location>> GetItems(WayPoint userLocation, int numItems = 3)
        {
            // Use Google Places API to search for Hospitals and Care Centers in the location passed.

            throw new NotImplementedException();
        }

        public async Task<string> GetMapImageUrl(WayPoint p1, WayPoint p2)
        {
            string mapUrlTemplate = "https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&key={2}";

            //TODO: Ensure to deal with failures/errors.
            string requestedMapUri = string.Format(mapUrlTemplate, p1.Coordinates, p2.Coordinates, MapsAPIKey);

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
            return blobUrl;
        }

        public async Task<string> GetRouteDistance(WayPoint p1, WayPoint p2)
        {
            string routeUrlTemplate = "https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&key={2}";
            string requestedRouteUri = string.Format(routeUrlTemplate, p1.Coordinates, p2.Coordinates, MapsAPIKey);

            string routeData = await client.GetStringAsync(requestedRouteUri);

            GoogleRouteResponse route = JsonConvert.DeserializeObject<GoogleRouteResponse>(routeData);

            throw new NotImplementedException();
        }
    }
}
