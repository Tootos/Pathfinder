using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Pathfinder.Classes;
using Pathfinder.Models;
using static Google.Rpc.Context.AttributeContext.Types;

namespace Pathfinder.Services.JourneyService
{
    public class JourneyService : IJourneyService
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<Journey> _collection;

        public JourneyService(IMongoClient mongoClient, IOptions<NemunaDBSettings> settings)
        {
            var dbSettings = settings.Value;
            _mongoClient = mongoClient;

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.DatabaseName);
            _collection = mongoDatabase.GetCollection<Journey>(dbSettings.RoutesCollectionName);
        }

        // CRUD Methods
        public async Task<ServiceResponse<Journey>> GetJourneyAsync(string id)
        {
           var response = new ServiceResponse<Journey>();

            //Check if ID format is valid
            if (!ObjectId.TryParse(id,out _))
            {
                response.Message = "Bad Request, ID format isn't valid";
                response.Success = false;
                return response;
            }

           var journey = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (journey == null)
            {
                response.Message = "Journey not found";
                response.Success = false;
            }
            else
            {
                response.Data = journey;
            }


                return response;
        }
            

        public async Task<ServiceResponse<Journey>> CreateJourneyAsync(Journey journey)
        {
            if (journey == null || string.IsNullOrEmpty(journey.EncodedRoutePath) || journey.Checkpoints.Count <2)
            {
                return new ServiceResponse<Journey>()
                {
                    Success = false,
                    Message = "Entity provided is missing important data",
                    Data = null
                };
            }
                       
            foreach(var checkpoint in journey.Checkpoints)
            {
                if (checkpoint.GeoLocationInput != null)
                {
                    checkpoint.GeoLocation = new GeoJsonPoint<GeoJson2DCoordinates>(

                        new GeoJson2DCoordinates(
                            checkpoint.GeoLocationInput.Latitude,
                            checkpoint.GeoLocationInput.Longtitude


                         ));
                }
            }

            await _collection.InsertOneAsync(journey);
            
            return new ServiceResponse<Journey>()
            {
                Data = journey
            };
        }

        public async Task<ServiceResponse<Journey>> UpdateJourneyAsync( Journey journey)
        {
            var response = new ServiceResponse<Journey>();
            //Check if ID format is valid
            if (!ObjectId.TryParse(journey.Id, out _))
            {
                
                return new ServiceResponse<Journey>()
                {
                    Message = "Bad Request, ID format isn't valid",
                    Success = false,
                };
            }

            if(journey.Checkpoints.Count < 2)
            {
                return new ServiceResponse<Journey>()
                {
                    Message = "Bad Request, missing checkpoints.",
                    Success = false,
                };
            }


            var filter = Builders<Journey>.Filter.Eq(j => j.Id, journey.Id);

            foreach (var checkpoint in journey.Checkpoints)
            {
                if (checkpoint.GeoLocationInput != null)
                {
                    checkpoint.GeoLocation = new GeoJsonPoint<GeoJson2DCoordinates>(

                        new GeoJson2DCoordinates(
                            checkpoint.GeoLocationInput.Latitude,
                            checkpoint.GeoLocationInput.Longtitude


                         ));
                }
            }


            var result = await _collection.ReplaceOneAsync(filter, journey);

                if (result.IsAcknowledged && result.ModifiedCount == 1)
                {
                return new ServiceResponse<Journey>()
                {
                    Data = journey
                };
               
                }


            return new ServiceResponse<Journey>()
            {
               Message = "Entity was not Updated",
                Success = false
            };
            
          
        }

        public async Task<ServiceResponse<bool>> DeleteJourneyAsync(string id)
        {

            if (!ObjectId.TryParse(id, out _))
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Message = "Bad Request, ID format isn't valid",
                    Success = false,

                };
            }

            var filter = Builders<Journey>.Filter.Eq(j => j.Id, id);
            var result = await _collection.DeleteOneAsync(filter);

            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return new ServiceResponse<bool>
                {
                    Data = true,
                    Message = "Entity Deleted"
                };
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Entity could not be deleted or already doesn't exist."
                };
            }
        }


        public async Task<ServiceResponse<bool>> CheckConnectionToDB()
        {
            try
            {

                var adminDatabase = _mongoClient.GetDatabase("admin");

                // Run the {ping: 1} command. This forces an immediate communication with the server.
                var result = await adminDatabase.RunCommandAsync((Command<BsonDocument>)"{ping: 1}");

                // The 'ping: 1' command returns { "ok": 1.0 } on success.
                // The simplest check: attempt to list the database names.
                // This operation requires a live connection to the server.


                return new ServiceResponse<bool>
                {
                    Success = result.Contains("ok") && result["ok"].AsDouble == 1.0

                };
                //await _mongoClient.ListDatabaseNamesAsync();
                //return true;
            }
            catch (Exception ex)
            {

                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"MongoDB connection check failed: {ex.Message}"
                };
                
            }
        }
    }
}
