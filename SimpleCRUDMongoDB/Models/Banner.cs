using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleCRUDMongoDB.Models
{
    public class Banner
    {
        [BsonId]
        [BsonRequired]
        public int Id { get; set; }

        [BsonElement("Html")]
        public string Html{ get; set; }

        [BsonElement("Created")]
        public DateTime Created { get; set; }

        [BsonElement("Modified")]
        public DateTime? Modified { get; set; }

    }
}