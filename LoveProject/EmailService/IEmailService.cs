namespace LoveProject.EmailService
{
    public interface IEmailService
    {
        public Task Send(string email, string subject, string message);

    }
}
