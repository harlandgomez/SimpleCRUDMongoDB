using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SimpleCRUDMongoDB.Models;

namespace SimpleCRUDMongoDB.Repository
{
    public class SequenceRepository
    {
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<Sequence> _collection;

        public SequenceRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<Sequence>(typeof(Sequence).Name);
        }

        public int GetSequenceValue(string sequenceName)
        {
            var filter = Builders<Sequence>.Filter.Eq(s => s.SequenceName, sequenceName);
            var update = Builders<Sequence>.Update.Inc(s => s.SequenceValue, 1);

            var result = _collection.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Sequence, Sequence> { IsUpsert = true, ReturnDocument = ReturnDocument.After });

            return result.SequenceValue;
        }
    }
}