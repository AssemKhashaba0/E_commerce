namespace E_commerce.utility
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

    }
}
