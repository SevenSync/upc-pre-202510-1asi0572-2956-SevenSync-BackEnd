namespace MaceTech.API.IAM.Infrastructure.Email.SendGrid.Configuration;

public class SendGridOptions
{
    public string ApiKey { get; set; }
    public string FromAddress { get; set; }
    public string FromName { get; set; }
}