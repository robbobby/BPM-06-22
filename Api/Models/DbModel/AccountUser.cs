using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.DbModel; 

public class AccountUser {
    [Key, Column(Order = 0)]
    public Guid AccountId { get; set; }
    [Key, Column(Order = 1)]
    public Guid UserId { get; set; }
    
    public Account Account { get; set; }
    public User User { get; set; }
    
    public string Role { get; set; }
}
