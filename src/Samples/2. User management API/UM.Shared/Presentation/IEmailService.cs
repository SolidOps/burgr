using System.Net.Mail;

namespace SolidOps.UM.Shared.Presentation;

public interface IEmailService
{
    void SendMail(MailMessage mailMessage);
}
