namespace Server.Core.Models.Entities.Entity.Base; 

public abstract class AuditableEntity
{
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}