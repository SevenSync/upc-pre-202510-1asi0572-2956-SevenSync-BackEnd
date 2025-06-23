using System.Text.Json.Serialization;
using MaceTech.API.IAM.Domain.Model.Enums;

namespace MaceTech.API.IAM.Domain.Model.Aggregates;

public class User(string uid, string email)
{
    //  @Properties
    public string Uid { get; } = uid;
    public string Email { get; private set; } = email;
    [JsonIgnore]
    public int TokenVersion { get; private set; } = 0;
    [JsonIgnore]
    public int Status { get; private set; } = (int)UserAccountState.Active;
    
    //  @Constructors
    public User() : this(string.Empty, string.Empty) { }
    
    //  @Methods
    public User UpdateUsername(string username)
    {
        this.Email = username;
        return this;
    }
    public void IncrementTokenVersion()
    {
        ++this.TokenVersion;
    }

    public void SetStatusToDeleted()
    {
        this.Status = (int)UserAccountState.Deleted;
    }
}