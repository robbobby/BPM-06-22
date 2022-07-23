using System.ComponentModel.DataAnnotations;
using Server.Core.Models.Entities.Entity.Base;

namespace Server.Core.Models.Entities.Entity; 

public class Job : AuditableEntity, IDbModel {
    [Key]
    public Guid Id { get; set; }
    public Project Project { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime? Deleted { get; set; }
}
