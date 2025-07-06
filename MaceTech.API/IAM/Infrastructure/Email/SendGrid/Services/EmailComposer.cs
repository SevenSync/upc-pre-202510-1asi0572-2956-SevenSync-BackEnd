using MaceTech.API.IAM.Application.External.Email.Models;
using MaceTech.API.IAM.Application.External.Email.Services;

namespace MaceTech.API.IAM.Infrastructure.Email.SendGrid.Services;

public class EmailComposer : IEmailComposer
{
    public EmailComponent ComposeWelcomeEmail(string destination, string verificationLink) 
    {
        const string subject = "¡Bienvenido a MaceTech!";
        var body = $"""

                            <!DOCTYPE html>
                            <html>
                            <head>
                              <meta charset="UTF-8">
                              <meta name="viewport" content="width=device-width, initial-scale=1.0">
                              <title>Verifica tu correo</title>
                            </head>
                            <body style="margin:0; padding:0; background-color:#f4f4f4;">
                              <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                  <td align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" width="600" style="background-color:#ffffff; padding:40px; border-radius:8px; font-family:Arial, sans-serif;">
                                      <tr>
                                        <td align="center" style="padding-bottom:20px;">
                                          <img src="https://cdn-icons-png.freepik.com/512/5610/5610944.png" alt="Success" width="100" height="100" style="display:block; border:0;" />
                                        </td>
                                      </tr>
                                      <tr>
                                        <td align="center" style="font-size:32px; font-weight:bold; color:#00C736; padding-bottom:20px;">
                                          MaceTech
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style="font-size:18px; font-weight:bold; color:#333333; padding-bottom:10px;">
                                          Bienvenido a MaceTech, ¡gracias por registrarte!
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style="font-size:16px; color:#555555; line-height:1.5; padding-bottom:30px;">
                                          Para empezar a disfrutar de nuestros servicios, por favor verifica tu dirección de correo electrónico haciendo clic en el botón de abajo:
                                        </td>
                                      </tr>
                                      <tr>
                                        <td align="center">
                                          <a href="{verificationLink}" target="_blank" style="display:inline-block; padding:12px 24px; font-size:16px; color:#ffffff; background-color:#333333; text-decoration:none; border-radius:4px;">
                                            Verificar correo
                                          </a>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>
                              </table>
                            </body>
                            </html>
                            
                    """;
        
        var ret = new EmailComponent(destination, subject, body);

        return ret;
    }

    public EmailComponent ComposePasswordResetEmail(string destination, string recoveryLink)
    {
      const string subject = "Recuperación de contraseña en MaceTech";
        var body = $"""

                            <!DOCTYPE html>
                            <html>
                            <head>
                              <meta charset="UTF-8">
                              <meta name="viewport" content="width=device-width, initial-scale=1.0">
                              <title>Recupera tu contraseña</title>
                            </head>
                            <body style="margin:0; padding:0; background-color:#f4f4f4;">
                              <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                  <td align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" width="600" style="background-color:#ffffff; padding:40px; border-radius:8px; font-family:Arial, sans-serif;">
                                      <tr>
                                        <td align="center" style="font-size:32px; font-weight:bold; color:#00C736; padding-bottom:20px;">
                                          MaceTech
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style="font-size:18px; font-weight:bold; color:#333333; padding-bottom:10px;">
                                          Recupera tu contraseña
                                        </td>
                                      </tr>
                                      <tr>
                                        <td style="font-size:16px; color:#555555; line-height:1.5; padding-bottom:30px;">
                                          Hemos recibido una solicitud para restablecer tu contraseña. Si no has solicitado este cambio, puedes ignorar este correo. De lo contrario, haz clic en el botón de abajo para restablecer tu contraseña:
                                        </td>
                                      </tr>
                                      <tr>
                                        <td align="center">
                                          <a href="{recoveryLink}" target="_blank" style="display:inline-block; padding:12px 24px; font-size:16px; color:#ffffff; background-color:#333333; text-decoration:none; border-radius:4px;">
                                            Recuperar contraseña
                                          </a>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>
                              </table>
                            </body>
                            </html>
                            
                    """;
        var ret = new EmailComponent(destination, subject, body);

        return ret;
    }
}