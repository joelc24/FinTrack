using FinTrack.WebApi.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FinTrack.WebApi.IntegrationTests
{
    [Collection(IntegrationCollection.Name)]
    public class AuthTests
    {
        private readonly HttpClient _httpClient;

        private sealed record LoginResponseTestDto(
            Guid AccountId,
            string AccountToken,
            List<ProfileSummaryTestDto> Profiles
        );

        private sealed record ProfileSummaryTestDto(Guid Id);

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

        [Fact]
        public async Task Register_WhenIsInvalid_ReturnsBadRequest()
        {

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", new {});
            var body = await response.Content.ReadAsStringAsync();

            Assert.True(
                HttpStatusCode.BadRequest == response.StatusCode,
                $"Se esperaba un 400 bad request y llego {(int)response.StatusCode}. Cuerpo: {body}"
            );

            var problems = JsonSerializer.Deserialize<ValidationProblemDetails>(body, JsonSerializerOptions.Web);

            Assert.NotNull(problems);
            Assert.Equal(problems.Status, (int)HttpStatusCode.BadRequest);
            
            Assert.NotNull(problems.Errors);
            Assert.True(problems.Errors.ContainsKey("Name"), "Se esperaba un error de validacion para el campo Name");
            Assert.True(problems.Errors.ContainsKey("LastName"), "Se esperaba un error de validacion para el campo LastName");
            Assert.True(problems.Errors.ContainsKey("UserName"), "Se esperaba un error de validacion para el campo UserName");
            Assert.True(problems.Errors.ContainsKey("Email"), "Se esperaba un error de validacion para el campo Email");
            Assert.True(problems.Errors.ContainsKey("Password"), "Se esperaba un error de validacion para el campo Password");

            Assert.NotEmpty(problems.Errors["Name"]);
            Assert.NotEmpty(problems.Errors["LastName"]);
            Assert.NotEmpty(problems.Errors["UserName"]);
            Assert.NotEmpty(problems.Errors["Email"]);
            Assert.NotEmpty(problems.Errors["Password"]);
        }

        
    }
}
