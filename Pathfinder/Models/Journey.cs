using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace Pathfinder.Models
{
    /// <summary>
    /// Journey represents the route that users take in cities
    /// </summary>
    public class Journey
    {
        //Entity Unique ID 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  = string.Empty;

        //Creator Id 
        [BsonElement("Creator Number")]
        [JsonPropertyName("cid")]
        public int Cid { get; set; }

        public string EncodedRoutePath { get; set; } = string.Empty;

        //Journey Display Title 
        [BsonElement("Title")]
        public string Title { get; set; } = string.Empty;
        //Journey Description 
        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;
        
        /*
        /// <summary>
        /// Location Elements: Start
        /// The latter elements contain the absolute geographical data
        /// WARNING: Due to their complex nested structure SWAGGER will have a difficulty rendering them 
        /// Solution: Exclude them from JSON payload and use Location Inputs for data transfer
        /// </summary>
        [BsonElement("Starting Location")]
        [JsonIgnore]
        public GeoJsonPoint<GeoJson2DCoordinates>? StartLocation { get; set; } = null;

        [BsonElement("End Location")]
        [JsonIgnore]
        public GeoJsonPoint<GeoJson2DCoordinates>? EndLocation { get; set; } = null;

        //Location Elements: End 

        /// <summary>
        /// Location Inputs: Start
        /// API proxies for the former two MongoDB Elements
        /// Used by Controllers for Input/Output 
        /// Ignored by MongoDB
        /// </summary>

        [BsonIgnore]
        public GeoPositionRecord? StartLocationInput { get; set; }
        [BsonIgnore]
        public GeoPositionRecord? EndLocationInput { get; set; }

        //Location Inputs: End 

        /// <summary>
        /// A checkpoint can be a Point of Interest or an important instruction
        /// added by the creator on a journey
        /// </summary>
        /// */
        public List<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
    }
}
