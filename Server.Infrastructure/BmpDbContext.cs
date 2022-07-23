using Microsoft.EntityFrameworkCore;
using Server.Core.Models.Entities.Entity;
using Server.Core.Models.Entities.Entity.Base;

namespace Domain;

public class BmpDbContext : DbContext {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AccountUser> AccountUsers { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Job> Jobs { get; set; }


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
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>()) {
            switch (entry.State) {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.DateUpdated = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.DateUpdated = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess) {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>()) {
            switch (entry.State) {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.DateUpdated = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.DateUpdated = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChanges(acceptAllChangesOnSuccess);
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