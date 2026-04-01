using Microsoft.AspNetCore.Mvc;
using Pathfinder.Models;

namespace Pathfinder.Services.JourneyService
{
    public interface IJourneyService
    {
        Task<ServiceResponse<Journey>> GetJourneyAsync(string id);
        Task<ServiceResponse<Journey>> CreateJourneyAsync(Journey newJourney);
        Task<ServiceResponse<Journey>> UpdateJourneyAsync(Journey updjourney);
        Task<ServiceResponse<bool>> DeleteJourneyAsync(string id);
        Task<ServiceResponse<bool>> CheckConnectionToDB();

        
    }
}
