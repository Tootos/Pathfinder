using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace Pathfinder.Models
{
    /// <summary>
    /// A checkpoint can be a Point of Interest or an important instruction
    /// added by the creator on a journey
    /// </summary>
    public class Checkpoint
    {

        [BsonElement("Order")]
        public int Step {  get; set; }
        public string Title { get; set; } = string.Empty;

        public string Information { get; set; } = string.Empty;

        /// <summary>
        /// GeoLocation
        /// The latter elements contain the absolute geographical data
        /// WARNING: Due to their complex nested structure SWAGGER will have a difficulty rendering them 
        /// Solution: Exclude them from JSON payload and use Location Inputs for data transfer
        /// </summary>
        [BsonElement("Starting Location")]
        [JsonIgnore]
        public GeoJsonPoint<GeoJson2DCoordinates>? GeoLocation { get; set; } = null;

        //Location Elements: End 

        /// <summary>
        /// GeoLocationInput
        /// API proxies for the former two MongoDB Elements
        /// Used by Controllers for Input/Output 
        /// Ignored by MongoDB
        /// </summary>
        [BsonIgnore]
        public GeoPositionRecord? LocationInput { get; set; }

        [BsonIgnore]
        public GeoPositionRecord? GeoLocationInput { get; set; }

    }
}
