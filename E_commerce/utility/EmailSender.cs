using System.Net.Mail;
using System.Net;
using E_commerce.utility;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("assemkhashaba0@gmail.com", "ceel deqk qjpm qqzu")
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("assemkhashaba0@gmail.com"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true // تحديد أن الرسالة هي HTML
        };
        mailMessage.To.Add(email);

        return client.SendMailAsync(mailMessage);
    }
}
