using FinTrack.WebApi.IntegrationTests.Fixtures;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FinTrack.WebApi.IntegrationTests.Helpers;
public static class CustomWebApplicationFactoryExtensions
{
    private static T Deserialize<T>(string body, string step) where T : class =>
        JsonSerializer.Deserialize<T>(body, JsonSerializerOptions.Web) ?? throw new InvalidOperationException($"{step} cuerpo vacio: {body}");
    public static async Task<AuthenticatedClientResult> CreateAuthenticatedClientAsync(this CustomWebApplicationFactory<Program> factory)
    {
        var client = factory.CreateClient();

        // 1 Create account and get a AccountId
        var requestRegister = new
        {
            Name = "John",
            LastName = "Doe",
            UserName = "JohnDoe" + Guid.NewGuid(),
            Email = "john_" + Guid.NewGuid() + "@example.com",
            Password = "abc123"
        };

        var responseRegister = await client.PostAsJsonAsync("/api/auth/register", requestRegister);

        await responseRegister.ReadBodyOrThrowAsync("Register account");

        // 2 Authenticate with credentials and get a token
        var responseLogin = await client.PostAsJsonAsync("/api/auth/login", new
        {
            requestRegister.Email,
            requestRegister.Password
        });

        var bodyLogin = await responseLogin.ReadBodyOrThrowAsync("Login account");

        var loginResponseData = Deserialize<LoginResult>(bodyLogin, "Login account");
        if (string.IsNullOrWhiteSpace(loginResponseData.AccountToken) || loginResponseData.Profiles is null)
            throw new InvalidOperationException($"Login account: respuesta incompleta. Cuerpo: {bodyLogin}");

        // 3 Set the token in the Authorization header of the client
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponseData.AccountToken);

        // 4 Get the list of profiles and select one to use for the tests
        var profile = loginResponseData.Profiles.FirstOrDefault();
        if(profile is null)
        {
            throw new InvalidOperationException("No profiles found for the account");
        }

        // 5 Set the token provided by the selected profile in the Authorization header of the client
        var responseSelectedProfile = await client.PostAsJsonAsync("/api/auth/select-profile", new
        {
            ProfileId = profile.Id
        });

        var bodySelectedProfile = await responseSelectedProfile.ReadBodyOrThrowAsync("Select profile");
        var selectedProfileData = Deserialize<SelectProfileResult>(bodySelectedProfile, "Select profile");
        if (string.IsNullOrWhiteSpace(selectedProfileData.Token))
            throw new InvalidOperationException($"Select profile: token no valido. Cuerpo: {bodySelectedProfile}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", selectedProfileData.Token);

        // 6 return the client
        return new AuthenticatedClientResult(client, loginResponseData.AccountId, profile.Id, selectedProfileData.Token);
    }
    private sealed record LoginResult(Guid AccountId, string AccountToken, List<ProfileInfo> Profiles);
    private sealed record ProfileInfo(Guid Id);
    private sealed record SelectProfileResult(string Token);
}

public sealed record AuthenticatedClientResult(HttpClient Client, Guid AccountId, Guid ProfileId, string Token);
