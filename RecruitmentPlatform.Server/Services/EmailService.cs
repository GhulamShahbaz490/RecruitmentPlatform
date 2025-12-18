using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RecruitmentPlatform.Server.DTOs;

namespace RecruitmentPlatform.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendInterviewCredentialsAsync(string toEmail, string interviewNumber, string pinCode)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Recruitment Platform", _config["Smtp:FromEmail"]));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Your Interview Credentials - Recruitment Platform";

                var body = $@"
                <html>
                <body>
                    <h2>Interview Scheduled Successfully!</h2>
                    <p>Your interview has been scheduled. Please use the credentials below to start your interview:</p>
                    <ul>
                        <li><strong>Interview Number:</strong> {interviewNumber}</li>
                        <li><strong>PIN Code:</strong> {pinCode}</li>
                    </ul>
                    <p>Please keep these credentials safe. You will need them to start your interview.</p>
                    <p>Good luck!</p>
                </body>
                </html>";

                message.Body = new TextPart("html") { Text = body };

                await SendEmailAsync(message);
                _logger.LogInformation($"Interview credentials sent to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send credentials to {toEmail}");
                throw;
            }
        }

        public async Task SendInterviewReceiptAsync(string toEmail, InterviewResultDto result)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Recruitment Platform", _config["Smtp:FromEmail"]));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = $"Interview Result - {result.Status}";

                var body = $@"
                <html>
                <body>
                    <h2>Interview Results</h2>
                    <p>Dear {result.ApplicantName},</p>
                    <p>Your interview results are ready:</p>
                    <ul>
                        <li><strong>Interview Number:</strong> {result.InterviewNumber}</li>
                        <li><strong>Total Score:</strong> {result.TotalScore}/{result.MaxPossibleScore}</li>
                        <li><strong>Percentage:</strong> {result.Percentage:F1}%</li>
                        <li><strong>Status:</strong> <span style='color: {(result.Status == "Pass" ? "green" : "red")}'>{result.Status}</span></li>
                    </ul>
                    <h3>Section Breakdown:</h3>
                    <ul>
                        {string.Join("", result.SectionScores.Select(s => $"<li><strong>{s.Key}:</strong> {s.Value.Score}/{s.Value.MaxScore} ({s.Value.Percentage:F1}%)</li>"))}
                    </ul>
                    {(result.Status == "Pass" ?
                            "<p><strong>Congratulations!</strong> You have been selected for the next round. Our team will contact you soon for a physical interview.</p>" :
                            "<p>We appreciate your effort. Unfortunately, you did not qualify for this position. You may apply for other positions in the future.</p>")}
                </body>
                </html>";

                message.Body = new TextPart("html") { Text = body };

                await SendEmailAsync(message);
                _logger.LogInformation($"Interview receipt sent to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send receipt to {toEmail}");
                throw;
            }
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["Smtp:Host"],
                int.Parse(_config["Smtp:Port"] ?? "587"),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_config["Smtp:Username"], _config["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
