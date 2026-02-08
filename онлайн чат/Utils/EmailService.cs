using System.Net;
using System.Net.Mail;

namespace ChatClient.Utils
{
    public static class EmailService
    {
        public static void Send(string to, string newPassword)
        {
            var mail = new MailMessage("your@gmail.com", to);
            mail.Subject = "Восстановление пароля";
            mail.Body = $"Новый пароль: {newPassword}";

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("your@gmail.com", "APP_PASSWORD");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
