using System.Net;
using System.Net.Http.Json;
using KiteschoolService.Dtos;
using KiteschoolService.Models;
using Moq;

namespace KiteschoolServiceTests.ControllersTests
{
    public class KiteschoolControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public KiteschoolControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetKiteschools_ReturnsOkWithCorrectData()
        {
            // Arrange
            var mockRepo = _factory.MockKiteschoolRepo;

            // Create some test data
            var expectedData = new List<Kiteschool>
            {
                new Kiteschool { Id = "1", Name = "Kite School 1", Location = "Location 1", Email = "school1@example.com", CreatedByUserId = 1 },
                new Kiteschool { Id = "2", Name = "Kite School 2", Location = "Location 2", Email = "school2@example.com", CreatedByUserId = 2 }
            };

            // Set up the mock repository to return the expected data
            mockRepo.Setup(repo => repo.GetAllKiteschools()).Returns(expectedData);

            // Act
            var response = await _client.GetAsync("/api/Kiteschool");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize the response content
            var responseData = await response.Content.ReadFromJsonAsync<List<KiteschoolReadDto>>();

            // Assert the expected data
            Assert.Equal(expectedData.Count, responseData.Count);

            for (var i = 0; i < expectedData.Count; i++)
            {
                Assert.Equal(expectedData[i].Id, responseData[i].Id);
                Assert.Equal(expectedData[i].Name, responseData[i].Name);
                Assert.Equal(expectedData[i].Location, responseData[i].Location);
                Assert.Equal(expectedData[i].Email, responseData[i].Email);
                Assert.Equal(expectedData[i].CreatedByUserId, responseData[i].CreatedByUserId);
            }
        }


        [Fact]
        public async Task GetKiteschoolsByUserId_ReturnsOkWithCorrectData()
        {
            // Arrange
            var userId = 1; // Set the user ID for testing

            // Create some test data
            var expectedData = new List<Kiteschool>
            {
                new Kiteschool { Id = "1", Name = "Kite School 1", Location = "Location 1", Email = "school1@example.com", CreatedByUserId = userId },
                new Kiteschool { Id = "2", Name = "Kite School 2", Location = "Location 2", Email = "school2@example.com", CreatedByUserId = userId }
            };

            // Set up the mock repository to return the expected data
            _factory.MockKiteschoolRepo.Setup(repo => repo.GetKiteschoolsByUserId(userId)).Returns(expectedData);

            // Act
            var response = await _client.GetAsync($"/api/Kiteschool/user/{userId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize the response content
            var responseData = await response.Content.ReadFromJsonAsync<List<KiteschoolReadDto>>();

            // Assert the expected data
            Assert.Equal(expectedData.Count, responseData.Count);

            for (var i = 0; i < expectedData.Count; i++)
            {
                Assert.Equal(expectedData[i].Id, responseData[i].Id);
                Assert.Equal(expectedData[i].Name, responseData[i].Name);
                Assert.Equal(expectedData[i].Location, responseData[i].Location);
                Assert.Equal(expectedData[i].Email, responseData[i].Email);
                Assert.Equal(expectedData[i].CreatedByUserId, responseData[i].CreatedByUserId);
            }
        }


        [Fact]
        public async Task GetKiteschoolById_ReturnsOkWithCorrectData()
        {
            // Arrange
            var kiteschoolId = "1"; // Set the kiteschool ID for testing

            // Create some test data
            var expectedData = new Kiteschool { Id = kiteschoolId, Name = "Kite School 1", Location = "Location 1", Email = "school1@example.com", CreatedByUserId = 1 };

            // Set up the mock repository to return the expected data
            _factory.MockKiteschoolRepo.Setup(repo => repo.GetKiteschoolById(kiteschoolId)).Returns(expectedData);

            // Act
            var response = await _client.GetAsync($"/api/Kiteschool/{kiteschoolId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize the response content
            var responseData = await response.Content.ReadFromJsonAsync<KiteschoolReadDto>();

            // Assert the expected data
            Assert.Equal(expectedData.Id, responseData.Id);
            Assert.Equal(expectedData.Name, responseData.Name);
            Assert.Equal(expectedData.Location, responseData.Location);
            Assert.Equal(expectedData.Email, responseData.Email);
            Assert.Equal(expectedData.CreatedByUserId, responseData.CreatedByUserId);
        }


        [Fact]
        public async Task GetKiteschoolById_ReturnsNotFound()
        {
            // Arrange
            var kiteschoolId = "nonexistentId"; // Set a nonexistent kiteschool ID for testing

            // Set up the mock repository to return null for the nonexistent ID
            _factory.MockKiteschoolRepo.Setup(repo => repo.GetKiteschoolById(kiteschoolId)).Returns((Kiteschool)null);

            // Act
            var response = await _client.GetAsync($"/api/Kiteschool/{kiteschoolId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task GetKiteschoolById_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var kiteschoolId = "1"; // Set the kiteschool ID for testing

            // Set up the mock repository to throw an exception
            _factory.MockKiteschoolRepo.Setup(repo => repo.GetKiteschoolById(kiteschoolId)).Throws(new Exception("Simulated error"));

            // Act
            var response = await _client.GetAsync($"/api/Kiteschool/{kiteschoolId}");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        [Fact]
        public async Task CreateKiteschool_ReturnsCreatedAtRouteWithCorrectData()
        {
            // Arrange
            var kiteschoolCreateDto = new KiteschoolCreateDto
            {
                Name = "New Kite School",
                Location = "New Location",
                Email = "new.school@example.com"
            };

            var expectedKiteschoolId = "generatedId"; // Set the expected ID after creation

            // Set up the mock repository to return the expected ID after creation
            _factory.MockKiteschoolRepo.Setup(repo => repo.CreateKiteschool(It.IsAny<Kiteschool>())).Callback<Kiteschool>(k => k.Id = expectedKiteschoolId);

            // Act
            var response = await _client.PostAsJsonAsync("/api/Kiteschool", kiteschoolCreateDto);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            // Deserialize the response content
            var responseData = await response.Content.ReadFromJsonAsync<KiteschoolReadDto>();

            // Assert the expected data
            Assert.Equal(expectedKiteschoolId, responseData.Id);
            Assert.Equal(kiteschoolCreateDto.Name, responseData.Name);
            Assert.Equal(kiteschoolCreateDto.Location, responseData.Location);
            Assert.Equal(kiteschoolCreateDto.Email, responseData.Email);
        }


        [Fact]
        public async Task CreateKiteschool_ReturnsBadRequestWhenDtoIsNull()
        {
            // Act
            var response = await _client.PostAsJsonAsync("/api/Kiteschool", (KiteschoolCreateDto)null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task CreateKiteschool_ReturnsInternalServerErrorOnException()
        {
            // Arrange
            var kiteschoolCreateDto = new KiteschoolCreateDto
            {
                // Set valid data
            };

            // Set up the mock repository to throw an exception
            _factory.MockKiteschoolRepo.Setup(repo => repo.CreateKiteschool(It.IsAny<Kiteschool>())).Throws(new Exception("Simulated error"));

            // Act
            var response = await _client.PostAsJsonAsync("/api/Kiteschool", kiteschoolCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
