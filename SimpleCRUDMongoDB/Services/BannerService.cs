using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SimpleCRUDMongoDB.Models;
using SimpleCRUDMongoDB.Repository;

namespace SimpleCRUDMongoDB.Services
{
    public class BannerService
    {
        private readonly IMongoCollection<Banner> _banners;
        private readonly IMongoDatabase _database;

        public BannerService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("BannerflowConnectionString"));
            _database = client.GetDatabase("BannerflowDB");
            _banners = _database.GetCollection<Banner>("Banner");
        }

        public List<Banner> Get()
        {
            return _banners.Find(b => true).ToList();
        }

        public Banner Get(int id)
        {
            return _banners.Find(b => b.Id == id).FirstOrDefault();
        }

        public Banner Create(Banner banner)
        {
            banner.Id = new SequenceRepository(_database).GetSequenceValue("Banner");
            banner.Created = DateTime.Now;
            _banners.InsertOne(banner);
            return banner;
        }

        public void Update(int id, Banner banner)
        {
            banner.Modified = DateTime.Now;
            _banners.ReplaceOne(b => b.Id == id, banner);
        }

        public void Remove(int id)
        {
            _banners.DeleteOne(b => b.Id == id);
        }


    }
}
