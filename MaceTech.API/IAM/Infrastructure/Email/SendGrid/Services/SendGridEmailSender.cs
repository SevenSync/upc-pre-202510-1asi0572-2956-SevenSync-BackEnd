using System.Net;
using System.Net.Mail;
using MaceTech.API.IAM.Application.External.Email.Models;
using MaceTech.API.IAM.Application.External.Email.Services;
using MaceTech.API.IAM.Infrastructure.Email.SendGrid.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MaceTech.API.IAM.Infrastructure.Email.SendGrid.Services;

public class SendGridEmailSender : IEmailSender
{
    //  @Variables
    private readonly SendGridClient _client;
    private readonly string _fromAddress;
    private readonly string _fromName;
    
    //  @Constructor
    public SendGridEmailSender(IOptions<SendGridOptions> options)
    {
        var o = options.Value;
        this._client = new SendGridClient(o.ApiKey);
        this._fromAddress = o.FromAddress;
        this._fromName = o.FromName;
    }
    
    //  @Functions
    public async Task SendEmailAsync(EmailComponent email)
    {
        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress(this._fromAddress, this._fromName),
            new EmailAddress(email.Destination),
            email.Subject,
            plainTextContent: null,
            htmlContent: email.Body
        );

        var response = await this._client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to send email");
        }
    }
}