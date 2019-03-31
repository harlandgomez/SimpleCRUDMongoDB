using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SimpleCRUDMongoDB.Models;

namespace SimpleCRUDMongoDB.IntegrationTests
{
    public class TestData
    {
        public static int TestId = -99;

        public static void CreateTestData()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var client = new MongoClient(config.GetConnectionString("BannerflowConnectionString"));
            var database = client.GetDatabase("BannerflowDB");
            var banners = database.GetCollection<Banner>("Banner");

            if (banners.CountDocuments(b => b.Id == TestId) != 0) return;
            var banner = new Banner
            {
                Id = TestId,
                Created = DateTime.Now,
                Html = "<html><title>Banner1</title><body>test data</body></html>"
            };
            banners.InsertOne(banner);
        }
    }
}