using Newtonsoft.Json;

namespace HealthBotLocations.Models
{
    /// <summary>
    /// DB Model for the obejcts stored in My Cosmos Collection.  
    /// Leverages the Point type for the GeoSpacial Queries via the Location property
    /// </summary>
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
        [JsonProperty("location")]
        public Point Location { get; set; }
        [JsonProperty("mapImageUrl")]
        public string MapImageUrl { get; set; }
    }
}
