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
        public async Task Register_WhenIsValid_ReturnLoggedInUser()
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

            var loggedInUser = JsonSerializer.Deserialize<LoginResponseTestDto>(body, JsonSerializerOptions.Web);
            Assert.NotNull(loggedInUser);
            Assert.NotEqual(Guid.Empty, loggedInUser.AccountId);
            Assert.False(string.IsNullOrWhiteSpace(loggedInUser.AccountToken));
            Assert.NotEmpty(loggedInUser.Profiles);
            Assert.All(loggedInUser.Profiles, p => Assert.NotEqual(Guid.Empty, p.Id));
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

        [Fact]
        public async Task Login_WhenDataIsValid_ReturnsSuccess()
        {
            var request = new
            {
                Name = "John",
                LastName = "Doe",
                UserName = "JohnDoe" + Guid.NewGuid(),
                Email = "john_" + Guid.NewGuid() + "@example.com",
                Password = "abc123"
            };

            var registerResponse = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            registerResponse.EnsureSuccessStatusCode();

            var loginRequest = new
            {
                request.Email,
                request.Password
            };

            var loginResponse = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            var body = await loginResponse.Content.ReadAsStringAsync();

            Assert.True(
                HttpStatusCode.OK == loginResponse.StatusCode,
                $"Se esperaba un 200 Ok y llego {(int)loginResponse.StatusCode}. Cuerpo: {body}"
            );

            var loginResponseDto = JsonSerializer.Deserialize<LoginResponseTestDto>(body, JsonSerializerOptions.Web);

            Assert.NotNull(loginResponseDto);
            Assert.NotEqual(Guid.Empty, loginResponseDto.AccountId);
            Assert.False(string.IsNullOrWhiteSpace(loginResponseDto.AccountToken));
            Assert.NotEmpty(loginResponseDto.Profiles);
            Assert.All(loginResponseDto.Profiles, l => Assert.NotEqual(Guid.Empty, l.Id));
        }
    }
}
