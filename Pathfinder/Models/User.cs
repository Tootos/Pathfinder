using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pathfinder.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty; 
        //public string Email { get; set; } = string.Empty;

        //public string Password { get; set; } = string.Empty;
    }
}
