using Api.Models.DbModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class BmpDbContext : DbContext {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AccountUser> AccountUsers { get; set; }
    

    public string DbPath { get; }

    public BmpDbContext(DbContextOptions<BmpDbContext> options) : base(options) {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
            SetAccountUsersKeys(modelBuilder);
    }

    public async Task Migrate() {
        await Database.MigrateAsync();
    }

    private void SetAccountUsersKeys(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AccountUser>()
            .HasKey(accountUser => new { accountUser.AccountId, accountUser.UserId });
        
        modelBuilder.Entity<AccountUser>()
            .HasOne(accountUser => accountUser.Account)
            .WithMany(account => account.AccountUsers)
            .HasForeignKey(accountUser => accountUser.AccountId);
        
        modelBuilder.Entity<AccountUser>()
            .HasOne(accountUser => accountUser.User)
            .WithMany(user => user.AccountUsers)
            .HasForeignKey(accountUser => accountUser.UserId);   
    }

}