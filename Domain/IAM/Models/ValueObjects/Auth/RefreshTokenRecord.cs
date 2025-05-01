using System.ComponentModel.DataAnnotations;

namespace Domain.IAM.Models.ValueObjects.Auth;

public class RefreshTokenRecord
{
    [Key]
    public int Id { get; set; }
    
    public long? UserId { get; set; }
    
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
    
    public DateTime? Expiration { get; set; }
    
    public DateTime? Created { get; set; }
    
    public bool? IsActive { get; set; }
}