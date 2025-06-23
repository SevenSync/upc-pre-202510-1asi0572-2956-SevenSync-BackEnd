using MaceTech.API.IAM.Application.External.Email.Models;

namespace MaceTech.API.IAM.Application.External.Email.Services;

public interface IEmailComposer
{
    public EmailComponent ComposeWelcomeEmail(string destination, string verificationLink);
    public EmailComponent ComposePasswordRecoveryEmail(string destination, string recoveryLink);
}