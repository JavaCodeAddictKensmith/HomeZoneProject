namespace HomeZone.Services.AuthAPI.Service.IService
{
    // IService/IEmailSender.cs
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

}
