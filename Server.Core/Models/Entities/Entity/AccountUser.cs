using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Core.Models.Entities.Entity.Base;

namespace Server.Core.Models.Entities.Entity; 

public class AccountUser : AuditableEntity, IDbModel {
    [Key, Column(Order = 0)]
    public Guid AccountId { get; set; }
    [Key, Column(Order = 1)]
    public Guid UserId { get; set; }
    
    public Account Account { get; set; }
    public User User { get; set; }
    
    public string Role { get; set; }
}
