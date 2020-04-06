namespace HealthBotLocations.Models
{
    public  class Point
    {
        public string Type => "Point";
        public double[] Coordinates { get; set; }       
    }
}
