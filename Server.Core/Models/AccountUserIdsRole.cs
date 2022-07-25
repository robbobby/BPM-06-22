namespace Server.Core.Models;

public class AccountUserIdsRole {
    public string Name { get; set; }
    public string Role { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
}
