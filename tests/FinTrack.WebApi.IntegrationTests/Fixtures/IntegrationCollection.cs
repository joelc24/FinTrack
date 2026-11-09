namespace FinTrack.WebApi.IntegrationTests.Fixtures;

[CollectionDefinition(Name)]
public class IntegrationCollection : ICollectionFixture<CustomWebApplicationFactory<Program>>
{
    public const string Name = "Integration";
}
