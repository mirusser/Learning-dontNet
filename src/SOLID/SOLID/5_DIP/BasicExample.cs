//Dependency Inversion Principle
//High-level modules should not depend on low-level modules.
//Abstractions should not depend on details.

namespace SOLID._5_DIP;

public interface IEmailSender
{
    // This interface defines a contract for sending emails.

    void SendEmail(string recipient, string subject, string body);
}

public class SmtpEmailSender : IEmailSender
{
    // This class is a concrete implementation of the
    // IEmailSender interface.

    public void SendEmail(string recipient, string subject, string body)
    {
        // Code to send an email using the SMTP protocol.
    }
}

public class NotificationService
{
    // This class depends on an IEmailSender interface
    // rather than a specific implementation.

    private readonly IEmailSender emailSender;

    public NotificationService(IEmailSender emailSender)
    {
        this.emailSender = emailSender;
    }

    public void SendNotification(string recipient, string message)
    {
        // Code to format the notification message.
        string subject = string.Empty;
        string body = message;

        emailSender.SendEmail(recipient, subject, body);
    }
}