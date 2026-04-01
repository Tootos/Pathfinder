using Microsoft.AspNetCore.Mvc;
using Pathfinder.Models;
using Pathfinder.Services.JourneyService;

namespace Pathfinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : Controller
    {
        private readonly IJourneyService _journeyService;
        public RoutesController(IJourneyService journeyService) 
        {
            _journeyService = journeyService;
         }

        [HttpPost("journey/post")]
        public async Task<ActionResult<ServiceResponse<Journey>>> CreateJourney([FromBody] Journey newJourney)
        {
            var response = await _journeyService.CreateJourneyAsync(newJourney);

            return Ok(response);
        }

        [HttpGet("journey/{id}")]
        public async Task<ActionResult<ServiceResponse<Journey>>> GetJourneyAsync(string id)
        {
            var response = await _journeyService.GetJourneyAsync(id);

            return Ok(response);
        }

        [HttpPut("journey/update/")]
        public async Task<ActionResult<ServiceResponse<Journey>>> UpdateJourney([FromBody] Journey updJourney)
        {
            var response = await _journeyService.UpdateJourneyAsync(updJourney);

            return Ok(response);

        }
        [HttpDelete("journey/delete/{id}")]
        public async Task<ActionResult<ServiceResponse<Journey>>> UpdateJourney(string id)
        {
            var response = await _journeyService.DeleteJourneyAsync(id);

            return Ok(response);

        }

        [HttpGet("status")]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckMongo()
        {
            var response = await _journeyService.CheckConnectionToDB();

            return Ok(response);
        }


        


    }
}
