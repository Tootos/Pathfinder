using Google.Maps.Places.V1;
using Google.Maps.Routing;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Pathfinder.Models;
using Pathfinder.Services.JourneyService;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pathfinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IConfiguration _configuration;

   //     private readonly PlacesClient _placesClient;

        public MapController(IConfiguration configuration)
        {
            _configuration = configuration;
          //  _placesClient = PlacesClient.Create();
        }

        [HttpGet("key")]
        public ActionResult<string> GetKey()
        {
            
                var apiKey = _configuration["Google:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    return StatusCode(500, "Key is missing");
                }
                return Ok(apiKey);
            
        }

        [HttpGet("mapid")]
        public ActionResult<string> GetMapId()
        {
            var mapId = _configuration["Google:MapId"];
            if (string.IsNullOrEmpty(mapId))
            {
                return StatusCode(500, "Key is missing");
            }
            return Ok(mapId);


        }

        [HttpPost("map/data")]
        public async Task<ActionResult<string>> GetMapData(double lat, double lng)
        {
            var apiKey = _configuration["GoogleApi:Key"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return StatusCode(500, "API key is not configured");
            }



            var googleMapUrl = $"https://maps.googleapis.com/maps/api/json?location={lat},{lng}&radius=500&key={apiKey}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(googleMapUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return Content(content, "application/json");
                    }
                    else
                    {
                        // Handle errors from Google's API
                        return StatusCode((int)response.StatusCode,
                                          $"Google Maps API error: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle network issues
                    return StatusCode(500, $"Network error connecting to Google: {ex.Message}");
                }

            }
        }

    }



    }

