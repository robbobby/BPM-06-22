using Api.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class BmpDbContext : DbContext {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    public BmpDbContext(DbContextOptions<BmpDbContext> options) : base(options) {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public async Task Migrate() {
        await Database.MigrateAsync();
    }
    
}