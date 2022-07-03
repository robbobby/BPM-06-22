using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.DbModel;

public class User : IDbModel {
    [Key]
    public Guid UserId { get; set; } = new Guid();
    public List<Account> Accounts { get; set; }
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(25)]
    public string FirstName { get; set; }
    [StringLength(25)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    public string EmailAddress { get; set; }
    [StringLength(36)]
    [ForeignKey(nameof(Account))]
    public Guid DefaultAccount { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public bool Disabled { get; set; } = false;
}
