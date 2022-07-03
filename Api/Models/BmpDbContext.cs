using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class BmpDbContext : DbContext {
    public DbSet<Account> Accounts { get; set; }

    public string DbPath { get; }
    public BmpDbContext(DbContextOptions<BmpDbContext> options) : base(options) {
    }
}

public class Account {
    public Guid AccountId { get; set; }
    public List<User> Users { get; set; }
    public DateTime DateCreated { get; set; }
    public string Plan { get; set; }
}

public class User {
    public Guid UserId { get; set; }
    public List<Account> Accounts { get; set; }
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(25)]
    public string FirstName { get; set; }
    [StringLength(25)]
    public string LastName { get; set; }
    [Required]
    public string EmailAddress { get; set; }
    [StringLength(36)]
    public Guid DefaultAccount { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastActive { get; set; }
    public bool Disabled { get; set; } = false;
}