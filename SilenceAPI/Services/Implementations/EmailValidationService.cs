using Microsoft.VisualBasic.FileIO;
using SilenceAPI.Models;
using SilenceAPI.Services.Abtracts;
using System.IO.Pipelines;
using System.Net;
using System.Text.RegularExpressions;

namespace SilenceAPI.Services.Implementations
{
    public class EmailValidationService : IEmailValidationService
    {
        private static List<EmailValidationMetrics> _emailValidationMetrics = new List<EmailValidationMetrics>();
        private static List<EmailData> _emailData = new List<EmailData>();
        public EmailValidationCounter ValidateEmailsFromFile(IFormFile file)
        {
            var validEmailsCount = 0;
            var invalidEmailsCount = 0;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (IsValidEmail(line))
                    {
                        SaveEmailInfo(line);
                        validEmailsCount++;
                    }
                    else
                    {
                        invalidEmailsCount++;
                    }
                }
            }

            var result = new EmailValidationCounter()
            {
                LoadId = Guid.NewGuid(),
                ValidEmailsCount = validEmailsCount,
                InvalidEmailsCount = invalidEmailsCount
            };

            var metrics = new EmailValidationMetrics()
            {
                LoadId = Guid.NewGuid(),
                ValidEmailsCount = validEmailsCount,
                InvalidEmailsCount = invalidEmailsCount,
                Created = DateTime.Now
            };

            _emailValidationMetrics.Add(metrics);

            return result;
        }

        public List<EmailValidationMetrics> GetMetrics()
        {
            return _emailValidationMetrics.Select(r => new EmailValidationMetrics
            {
                LoadId = r.LoadId,
                ValidEmailsCount = r.ValidEmailsCount,
                InvalidEmailsCount = r.InvalidEmailsCount,
                Created = r.Created
            }).ToList();
        }

        public List<EmailData>SearchEmail(string searchText)
        {
             return _emailData.Where(e => e.Email.Contains(searchText) 
                                       || e.Username.Contains(searchText) 
                                       || e.Domain.Contains(searchText) 
                                       || e.Tld.Contains(searchText)).ToList();
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private void SaveEmailInfo(string email)
        {
            string[] parts = email.Split('@');
            string username = parts[0];
            string[] domainParts = parts[1].Split('.');
            string domain = domainParts[0];
            string tld = domainParts[1];

            _emailData.Add(new EmailData()
            {
                Email = email,
                Username = username,
                Domain = domain,
                Tld = tld
            });
        }
    }
}
