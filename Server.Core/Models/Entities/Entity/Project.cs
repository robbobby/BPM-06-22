using System.ComponentModel.DataAnnotations;
using Server.Core.Models.Entities.Entity.Base;

namespace Server.Core.Models.Entities.Entity; 

public class Project : AuditableEntity, IDbModel {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}
