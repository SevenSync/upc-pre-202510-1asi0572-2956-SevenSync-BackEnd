using System.ComponentModel.DataAnnotations;
using Domain.IAM.Models.Enums;
using Domain.IAM.Models.ValueObjects.User;

namespace Domain.IAM.Models.Aggregates;

public class User
{
    [Key]
    public UserId UserId { get; init; } = null!;
    public FullName FullName { get; set; } = null!;
    public Email Email { get; set; } = null!;
    public HashPassword HashPassword { get; set; } = null!;
    public int AssignedRole { get; set; } = (int) Role.User;
    public int CurrentStatus { get; set; } = (int) Status.Active;
    
    public void ChangeFullName(string name, string lastName)
    {
        this.FullName = new FullName(name, lastName);
    }
}