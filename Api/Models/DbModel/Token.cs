using System.ComponentModel.DataAnnotations;

namespace Api.Models.DbModel; 

public class Token : IDbModel {
    [Key]
    public int Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public User User { get; set; }
}
