using System.Collections.Generic;

namespace HealthBotLocations.Models
{
    public class ResourceSet
    {
        public int EstimatedTotal { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}
