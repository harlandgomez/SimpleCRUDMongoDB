using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleCRUDMongoDB.Models
{
    public class Sequence
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string SequenceName { get; set; }
        [BsonElement("Value")]
        public int SequenceValue { get; set; }
    }
}