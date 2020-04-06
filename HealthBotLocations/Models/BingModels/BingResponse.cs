using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBotLocations.Models
{
    public class BingResponse
    {
        public string authenticationResultCode { get; set; }
        public IEnumerable<ResourceSet> ResourceSets { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}
