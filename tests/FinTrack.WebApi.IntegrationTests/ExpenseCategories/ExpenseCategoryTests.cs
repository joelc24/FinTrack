using FinTrack.WebApi.IntegrationTests.Fixtures;
using FinTrack.WebApi.IntegrationTests.Helpers;
using System.Net;
using System.Text.Json;

namespace FinTrack.WebApi.IntegrationTests.ExpenseCategories;

[Collection(IntegrationCollection.Name)]
public class ExpenseCategoryTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private sealed record ExpenseCategoryTestDto(
        Guid Id,
        IReadOnlyList<ExpenseSubCategoryTestDto> SubCategories
    );

    private sealed record ExpenseSubCategoryTestDto(
        Guid Id
    );

    public ExpenseCategoryTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategories_WhenAuthenticated_ReturnsSeededCategories()
    {
        var auth = await _factory.CreateAuthenticatedClientAsync();

        var response = await auth.Client.GetAsync("/api/expense-categories");
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Se esperaba 200 Ok y llego {(int)response.StatusCode}. Cuerpo: {body}"
        );

        var categories = JsonSerializer.Deserialize<List<ExpenseCategoryTestDto>>(body, JsonSerializerOptions.Web);
        
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.All(categories, c => Assert.NotEqual(Guid.Empty, c.Id));
        Assert.Contains(categories, c => c.SubCategories is { Count: > 0 });

    }

    [Fact]
    public async Task GetCategories_WhenNotAuthenticated_ReturnsUnauhtorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/expense-categories");

        Assert.True(
            HttpStatusCode.Unauthorized == response.StatusCode,
            "Se esperaba que fallara al no tener autorizacion"
        );
    }

}
