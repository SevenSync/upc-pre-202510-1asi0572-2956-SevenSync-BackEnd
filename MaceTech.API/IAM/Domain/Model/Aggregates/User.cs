using System.Text.Json.Serialization;

namespace MaceTech.API.IAM.Domain.Model.Aggregates;

public class User(string email, string passwordHash)
{
    //  @Properties
    public long Id { get; }
    public string Email { get; private set; } = email;
    [JsonIgnore]
    public string PasswordHash { get; private set; } = passwordHash;
    
    //  @Constructors
    public User() : this(string.Empty, string.Empty) { }
    
    //  @Methods
    public User UpdateUsername(string username)
    {
        this.Email = username;
        return this;
    }
}