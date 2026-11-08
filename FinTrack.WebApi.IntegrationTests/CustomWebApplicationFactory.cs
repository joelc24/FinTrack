using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace FinTrack.WebApi.IntegrationTests;


public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithDatabase("FinTrackTestDb")
        .Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _msSqlContainer.GetConnectionString());
        builder.UseSetting("Jwt:SecretKey", "fdWRqpOnM87CvEAK051NKMm7WUfyc+tUUeFIdXIpiZs=");
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
