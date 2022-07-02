using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Setup;

public static class DbSetup {
    public static void SetDbContext(WebApplicationBuilder builder) {
        var port = builder.Configuration.GetSection("Database:Port").Value;
        var username = builder.Configuration.GetSection("Database:Username").Value;
        var password = builder.Configuration.GetSection("Database:Password").Value;
        var host = builder.Configuration.GetSection("Database:Host").Value;
        var database = builder.Configuration.GetSection("Database:Database").Value;
        
        builder.Services.AddDbContext<BloggingContext>(options => {
            options.UseNpgsql(
                $"Host={host};Database={database};Username={username};Password={password}");
        });
    }
}
