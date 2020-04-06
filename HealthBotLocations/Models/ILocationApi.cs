using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthBotLocations.Models
{
    public interface ILocationApi
    {
        Task<IEnumerable<Location>> GetItems(WayPoint userLocation, int numItems = 3);

        Task<string> GetMapImageUrl(WayPoint p1, WayPoint p2);

        Task<string> GetRouteDistance(WayPoint p1, WayPoint p2);
    }
}
