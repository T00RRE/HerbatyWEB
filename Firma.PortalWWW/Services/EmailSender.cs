using System.Net;
using System.Net.Mail;

namespace Firma.PortalWWW.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task WyslijEmail(string doKogo, string temat, string trescHTML)
        {
            var settings = _configuration.GetSection("EmailSettings");

            // Pobieramy dane z appsettings.json
            var host = settings["Host"];
            var port = int.Parse(settings["Port"]);
            var smtpUser = settings["SmtpUser"];
            var smtpPass = settings["SmtpPass"];
            var nadawcaEmail = settings["NadawcaEmail"];
            var nadawcaNazwa = settings["NadawcaNazwa"];

            using (var klient = new SmtpClient(host, port))
            {
                klient.EnableSsl = true;
                klient.Credentials = new NetworkCredential(smtpUser, smtpPass);

                var wiadomosc = new MailMessage
                {
                    From = new MailAddress(nadawcaEmail, nadawcaNazwa),
                    Subject = temat,
                    Body = trescHTML,
                    IsBodyHtml = true
                };

                wiadomosc.To.Add(doKogo);

                try
                {
                    await klient.SendMailAsync(wiadomosc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd wysyłki maila: {ex.Message}");
                    throw; 
                }
            }
        }
    }
}