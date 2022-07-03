using System.Data.Common;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Setup;

public static class DbSetup {
    public static void SetDbContext(WebApplicationBuilder builder) {
        var port = builder.Configuration.GetSection("Database:Port").Value;
        var username = builder.Configuration.GetSection("Database:Username").Value;
        var password = builder.Configuration.GetSection("Database:Password").Value;
        var host = builder.Configuration.GetSection("Database:Host").Value;
        var database = builder.Configuration.GetSection("Database:Database").Value;

        var connectionString = new NpgsqlConnectionStringBuilder();
        int.TryParse(port, out var portNumber);
        connectionString.PersistSecurityInfo = true;
        connectionString.Passfile = "./pgpass.conf";
        connectionString.Port = portNumber;
        connectionString.Password = password;
        connectionString.Username = username;
        connectionString.Host = host;
        connectionString.Database = database;

        builder.Services.AddDbContext<BmpDbContext>(options => {
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
