using HealthBotLocations.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBotLocations.Helpers
{
    public class NearbyHelper
    {
        public static async Task<IEnumerable<Location>> GetNearbyLocations(ILocationApi api, WayPoint userLocation)
        {
            List<Location> locationResults = new List<Location>();

            locationResults = (await api.GetItems(userLocation)).ToList();

            foreach (Location l in locationResults)
            {
                WayPoint destinationLocation = new WayPoint
                {
                    Latitude = l.Point.Coordinates[0],
                    Longitude = l.Point.Coordinates[1]
                };

                string mapUri = await api.GetMapImageUrl(userLocation, destinationLocation);
                string routeDistance = await api.GetRouteDistance(userLocation, destinationLocation);

                l.MapUri = mapUri;
                l.Distance = routeDistance;
            }

            return locationResults;
        }
    }
}
