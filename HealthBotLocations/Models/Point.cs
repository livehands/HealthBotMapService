using Newtonsoft.Json;

namespace HealthBotLocations.Models
{
    /// <summary>
    /// GeoJSON formated Point object used to enable GeoSpatial Queries in Cosmos.
    /// </summary>
    public class Point
    {
        [JsonProperty("type")]
        public string Type => "Point";
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }

        public Point()
        {

        }

        public Point(double latitude, double longitude)
        {
            Coordinates = new double[] { latitude, longitude };
        }
    }
}
