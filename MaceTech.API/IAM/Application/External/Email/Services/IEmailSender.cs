using MaceTech.API.IAM.Application.External.Email.Models;

namespace MaceTech.API.IAM.Application.External.Email.Services;

public interface IEmailSender
{
    public Task SendEmailAsync(EmailComponent email);
}