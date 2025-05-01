namespace Presentation.IAM.Response;

public class UserAuthenticationResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Result { get; set; }
    public string Message { get; set; } = string.Empty;
}