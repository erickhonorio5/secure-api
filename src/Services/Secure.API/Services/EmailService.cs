using Secure.API.Interfaces;

namespace Secure.API.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string email, string code)
    {
        var emailMessage = new StringBuilder();
        emailMessage.AppendLine("<html>");
        emailMessage.AppendLine("<body>");
        emailMessage.AppendLine($"<p>Querido {email},<p>");
        emailMessage.AppendLine("<p>Obrigado por se registrar! Para verificar seu e-mail, use o código de verificação abaixo:</p>");
        emailMessage.AppendLine($"<h2>Código de verificação: {code}</h2>");
        emailMessage.AppendLine("<br>");
        emailMessage.AppendLine("<h3> Serviço de Autenticação </h3>");
        emailMessage.AppendLine("</body>");
        emailMessage.AppendLine("</html>");

        string message = emailMessage.ToString();
        var _email = new MimeMessage();
        _email.To.Add(MailboxAddress.Parse(email)); // O email do usuário
        _email.From.Add(MailboxAddress.Parse("MS_5maL8u@trial-yzkq340v280ld796.mlsender.net")); // Remetente
        _email.Subject = "Confirmação de e-mail";
        _email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.mailersend.net", 587, MailKit.Security.SecureSocketOptions.StartTls); // Configurar SMTP
        smtp.Authenticate("MS_5maL8u@trial-yzkq340v280ld796.mlsender.net", "mssp.LxxvBqm.k68zxl2ezxklj905.jUn0Q6P");
        await smtp.SendAsync(_email);
        smtp.Disconnect(true);
    }
}
