using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace WeatherAPI.Models
{
    public class GeolocationContext : DbContext
    {
        public GeolocationContext(DbContextOptions<GeolocationContext> options)
            : base(options)
        {
        }
        public DbSet<GeolocationDTO> Geolocation { get; set; } = null!;
    }
}
