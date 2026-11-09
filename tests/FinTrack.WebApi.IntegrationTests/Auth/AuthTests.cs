using FinTrack.WebApi.IntegrationTests.Fixtures;
using System.Net;
using System.Text.Json;

namespace FinTrack.WebApi.IntegrationTests
{
    [Collection(IntegrationCollection.Name)]
    public class AuthTests
    {
        private readonly HttpClient _httpClient;

        public AuthTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_WhenIsValid_ReturnAccountId()
        {
            var request = new
            {
                Name = "John",
                LastName = "Doe",
                UserName = "JohnDoe" + Guid.NewGuid(),
                Email = "john_" + Guid.NewGuid() + "@example.com",
                Password = "abc123"
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            var body = await response.Content.ReadAsStringAsync();

            Assert.True(
                HttpStatusCode.OK == response.StatusCode,
                $"Se esperaba 200 y llegó {(int)response.StatusCode}. Cuerpo: {body}"
            );

            var accountId = JsonSerializer.Deserialize<Guid>(body);
            Assert.NotEqual(Guid.Empty, accountId);
        }
    }
}
