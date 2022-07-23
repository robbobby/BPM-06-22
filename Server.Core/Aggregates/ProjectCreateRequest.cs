namespace Server.Core.Aggregates;

public class ProjectCreateRequest {
    public string Name { get; set; }
    public string Description { get; set; }
    public string? AccountId { get; set; }
}
