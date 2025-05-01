namespace Presentation.IAM.Response;

public class UserResponse
{
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastNames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}