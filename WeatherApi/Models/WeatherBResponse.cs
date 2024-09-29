namespace WeatherApi.Models
{
    public class WeatherBResponse
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public double Fahrenheit { get; set; } 
    }
}
