using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.DbModel;

public class Account : IDbModel {
    [Key]
    public Guid AccountId { get; set; } = new Guid();

    [Required(ErrorMessage = "Name is required")]
    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
    public string Name { get; set; } = "My Account";
    
    public List<User> Users { get; set; } = new List<User>();
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string Plan { get; set; } = "free";
}