using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BoxOffice.Core.Data.Entities
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Seat { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public string SpectacleId { get; set; }
        public Spectacle Spectacle { get; set; }
    }
}
