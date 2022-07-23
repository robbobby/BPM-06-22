using System.ComponentModel.DataAnnotations;
using Server.Core.Models.Entities.Entity.Base;

namespace Server.Core.Models.Entities.Entity;

public class Account : AuditableEntity, IDbModel {
    [Key]
    public Guid Id { get; set; } = new Guid();

    [Required(ErrorMessage = "Name is required")]
    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
    public string Name { get; set; } = "My Account";
    
    public List<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
    public DateTime DateCreated { get; set; }
    public string Plan { get; set; } = "free";

    public List<Project> Projects { get; set; }
}
