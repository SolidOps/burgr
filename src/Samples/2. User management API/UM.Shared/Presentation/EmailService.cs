using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.SubZero;
using System.Net;
using System.Net.Mail;

namespace SolidOps.UM.Shared.Presentation;

public class EmailService : IEmailService
{
    private readonly IExtendedConfiguration configuration;
    private readonly ILogger<EmailService> logger;

    public EmailService(IExtendedConfiguration configuration, ILoggerFactory loggerFactory)
    {
        this.configuration = configuration;
        this.logger = loggerFactory.CreateLogger<EmailService>();
    }

    public void SendMail(MailMessage mailMessage)
    {
        if (string.IsNullOrEmpty(this.configuration.BurgrConfiguration.SmtpServer))
        {
            this.logger.LogInformation("Mock mail: " + Serializer.Serialize(mailMessage));
            return;
        }

        this.logger.LogDebug("SendMail using " + this.configuration.BurgrConfiguration.SmtpServer);
        SmtpClient client = new SmtpClient(this.configuration.BurgrConfiguration.SmtpServer, this.configuration.BurgrConfiguration.SmtpPort);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(this.configuration.BurgrConfiguration.SmtpUserName, this.configuration.BurgrConfiguration.SmtpPassword);
        client.Send(mailMessage);
        client.Dispose();
    }
}
