namespace HealthBotLocations.Models
{
    public class WayPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Coordinates => $"{Latitude},{Longitude}";
    }
}
