using Newtonsoft.Json;

namespace HealthBotLocations.Models
{
    public class Hospital
    {
        [JsonProperty("facilityId")]
        public string FacilityId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("Zip")]
        public string Zip { get; set; }
        [JsonProperty("county")]
        public string County { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("hospitalType")]
        public string HospitalType { get; set; }
        [JsonProperty("ownership")]
        public string Ownership { get; set; }
        [JsonProperty("emergencyServices")]
        public string EmergencyServices { get; set; }
        [JsonProperty("rating")]
        public string Rating { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("mapImageUrl")]
        public string MapImageUrl { get; set; }
    }
}
