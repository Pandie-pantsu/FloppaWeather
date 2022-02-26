#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Models;
using Newtonsoft.Json;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationDTOController : ControllerBase
    {
        private readonly GeolocationContext _context;
        public GeolocationDTOController(GeolocationContext context)
        {
            _context = context;
        }

        // GET: api/GeolocationDTO
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GeolocationDTO>>> City()
        {
            var loc = _context.Geolocation
                .FirstOrDefault();
            //await _context.Geolocation
            //  .Select(x => GeoToDTO(x))
            //.ToArrayAsync();

            using (var client = new HttpClient())
            {
                try
                {
                    string Secret = "{YOUR API KEY HERE}";
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"/data/2.5/weather?lat="+loc.Latitude+"&lon="+loc.Longitude+"&appid="+Secret+"&units=imperial");
                    response.EnsureSuccessStatusCode();
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);

                    return Ok(new
                    {
                        Temp = rawWeather.Main.Temp,
                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Description)),
                        City = rawWeather.Name,
                        Id = string.Join(",", rawWeather.Weather.Select(x=>x.id)),
                        Icon = string.Join(",", rawWeather.Weather.Select(x=>x.icon)),
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Oopsie poopsie! Error getting weather from the API: {httpRequestException}");
                }

            }
        }
        
        // GET: api/GeolocationDTO/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeolocationDTO>> GetGeolocationDTO(int id)
        {
            var geolocationDTO = await _context.Geolocation.FindAsync(id);

            if (geolocationDTO == null)
            {
                return NotFound();
            }

            return geolocationDTO;
        }

        
        // PUT: api/GeolocationDTO/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeolocationDTO(int id, GeolocationDTO geolocationDTO)
        {
            if (id != geolocationDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(geolocationDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeolocationDTOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GeolocationDTO
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeolocationDTO>> PostGeolocationDTO(GeolocationDTO geolocationDTO)
        {
            int id = 1;
            var geoLocation = new GeolocationDTO
            {
                Latitude = geolocationDTO.Latitude,
                Longitude = geolocationDTO.Longitude,
            };

            var update = await _context.Geolocation.FindAsync(id);
            _context.Geolocation.Update(geolocationDTO);

            if (update != null)
            {
                update.name = geoLocation.name;
                update.Latitude = geolocationDTO.Latitude;
                update.Longitude = geolocationDTO.Longitude;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(City),
               // new { id = geolocationDTO.Id },
                GeoToDTO(geoLocation));
           

        }

        // DELETE: api/GeolocationDTO/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeolocationDTO(int id)
        {
            var geolocationDTO = await _context.Geolocation.FindAsync(id);
            if (geolocationDTO == null)
            {
                return NotFound();
            }

            _context.Geolocation.Remove(geolocationDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        private bool GeolocationDTOExists(int id)
        {
            return _context.Geolocation.Any(e => e.Id == id);
        }
       
        private static GeolocationDTO GeoToDTO(GeolocationDTO geoLocation) =>
          new GeolocationDTO
            {
               // Id = geoLocation.Id,
                name = geoLocation.name,
                Latitude = geoLocation.Latitude,
                Longitude = geoLocation.Longitude,
            };

        private static OpenWeatherResponse WeatherToDTO(OpenWeatherResponse openWeather) =>
            new OpenWeatherResponse
            {
                Name = openWeather.Name,
                Weather = openWeather.Weather,
                Main = openWeather.Main,
            };
    }
}
