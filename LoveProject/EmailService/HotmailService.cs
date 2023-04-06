using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace LoveProject.EmailService
{
    public class HotmailService:IEmailService
    { 
        private string _username;
        private int _port;
        private string _password;
        private string _host;
        private bool _enableSSL;

        public HotmailService(string username, int port, string password, string host,bool enableSSl)
        {
            _username = username;
            _port = port;
            _password = password;
            _host = host;
            _enableSSL = enableSSl;
        }

        public Task Send(string email,string subject ,string message) {

            var smtp = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSL
            };

            return smtp.SendMailAsync(
                new MailMessage(_username, email, subject, message)
                {
                    IsBodyHtml = true
                });
        }

    }
}
