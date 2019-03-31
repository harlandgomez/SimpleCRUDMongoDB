using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SimpleCRUDMongoDB.Models;
using Xunit;

namespace SimpleCRUDMongoDB.IntegrationTests
{
    public class BannerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BannerControllerTests(WebApplicationFactory<Startup> factory)
        {
            //Arrange
            _client = factory.CreateClient();
            TestData.CreateTestData();
        }

        [Fact]
        public async Task GetAll_ResponseShouldContainValues()
        {
            
            // Act
            var response = await _client.GetAsync("api/banner");
            response.EnsureSuccessStatusCode();            
            var responseString = await response.Content.ReadAsStringAsync();
            var banners = JsonConvert.DeserializeObject<List<Banner>>(responseString);
            
            // Assert            
            banners.Should().NotBeNull();
            banners.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task GetOne_ResponseShouldContainValues()
        {
            //Arrange
            var expectedId = TestData.TestId;

            // Act
            var response = await _client.GetAsync($"api/banner/{TestData.TestId}");
            response.EnsureSuccessStatusCode(); 
            var responseString = await response.Content.ReadAsStringAsync();
            var actualBanner = JsonConvert.DeserializeObject<Banner>(responseString);

            // Assert
            Assert.Equal(expectedId, actualBanner.Id);
        }

        [Fact]
        public async Task Post_ResponseShouldCreateBanner()
        {
            //Arrange
            var newBanner = new Banner()
            {
                Html = "hello"
            };

            var serializeObject = JsonConvert.SerializeObject(newBanner);
            var contentData = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            // Act
            using (var response = await _client.PostAsync("api/Banner", contentData))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var actualBanner = JsonConvert.DeserializeObject<Banner>(responseString);

                //Assert that it is created
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                //Assert value of Html
                Assert.Equal(newBanner.Html, actualBanner.Html);
                //Assert that Created is > min value
                actualBanner.Created.Should().BeLessThan(TimeSpan.MinValue);
                //Assert that Modified is null
                Assert.Null(actualBanner.Modified);
            }
        }

        [Fact]
        public async Task Put_ResponseShouldUpdateBanner()
        {
            //Arrange
            var updatedBanner = new Banner()
            {
                Id = TestData.TestId,
                Html = "<html><title>Banner1</title><body>Modified Data</body></html>"
            };

            var serializeObject = JsonConvert.SerializeObject(updatedBanner);
            var contentData = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"api/banner/{TestData.TestId}", contentData);
            response.EnsureSuccessStatusCode();


            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            response = await _client.GetAsync($"api/banner/{TestData.TestId}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var actualBanner = JsonConvert.DeserializeObject<Banner>(responseString);

            //Assert that Html we updated is the same
            Assert.Equal(actualBanner.Html, updatedBanner.Html);
            //Assert that Modified is NOT null
            Assert.NotNull(actualBanner.Modified);
        }

        [Fact]
        public async Task Delete_ResponseShouldDeleteBanner()
        {
            var response = await _client.DeleteAsync($"api/Banner/{TestData.TestId}");
            
            response.EnsureSuccessStatusCode();

            //Assert the No Content response
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            response = await _client.GetAsync($"api/banner/{TestData.TestId}");

            //Assert that it does not exist anymore
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetBanner_ResponseShouldContainValidHtml()
        {
            // Act
            var response = await _client.GetAsync($"api/banner/GetBanner/{TestData.TestId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(responseString);

            //Assert that there is no Parse Errors which means it's a valid HTML
            Assert.False(doc.ParseErrors.Any());
        }
    }
}
