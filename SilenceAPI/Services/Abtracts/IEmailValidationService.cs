using SilenceAPI.Models;

namespace SilenceAPI.Services.Abtracts
{
    public interface IEmailValidationService
    {
        EmailValidationCounter ValidateEmailsFromFile(IFormFile file);
        List<EmailValidationMetrics> GetMetrics();
        List<EmailData> SearchEmail(string searchText);

    }
}
