using Api.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : BmpDbContext {
    private readonly TestcontainerDatabase _container;

    public IntegrationTestFactory() {
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration {
                Database = "test_db",
                Username = "postgres",
                Password = "postgres",
            })
            .WithImage("postgres:11")
            .WithCleanUp(true)
            .Build();
    }

    protected override async void ConfigureWebHost(IWebHostBuilder builder) {
        
        builder.ConfigureTestServices(services => {
            services.AddDbContext<TDbContext>(options => { options.UseNpgsql(_container.ConnectionString); });
        });
        var thing = builder.Build();
        await thing.Services.GetService<TDbContext>()!.Migrate();

    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}
