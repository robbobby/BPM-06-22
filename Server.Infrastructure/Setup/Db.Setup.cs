using System.Configuration;
using System.Data.Common;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Api.Setup;

public static class DbSetup {
    public static void SetDbContext(IConfiguration config, IServiceCollection services) {
        var port = config.GetSection("Database:Port").Value;
        var username = config.GetSection("Database:Username").Value;
        var password = config.GetSection("Database:Password").Value;
        var host = config.GetSection("Database:Host").Value;
        var database = config.GetSection("Database:Database").Value;

        var connectionString = new NpgsqlConnectionStringBuilder();
        int.TryParse(port, out var portNumber);
        connectionString.PersistSecurityInfo = true;
        connectionString.Passfile = "./pgpass.conf";
        connectionString.Port = portNumber;
        connectionString.Password = password;
        connectionString.Username = username;
        connectionString.Host = host;
        connectionString.Database = database;

        services.AddDbContext<BmpDbContext>(options => {
            options.UseNpgsql(String.Format(
                "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                host,
                username,
                database,
                port,
                password));
        });
    }
}
