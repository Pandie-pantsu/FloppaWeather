using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WeatherAPI.Models
{
    public class GeolocationDTO
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? name { get; set; }
    }

    public class OpenWeatherResponse
    {
        public string? Name { get; set; }
        public IEnumerable<WeatherDescription>? Weather { get; set; }
        public Main? Main { get; set; }

    }

    public class WeatherDescription
    {
        public string? Main { get; set; }
        public string? Description { get; set; }
        public string? id { get; set; }
        public string? icon { get; set; }

    }
    public class Main
    {
        public string? Temp { get; set; }
    }
}


