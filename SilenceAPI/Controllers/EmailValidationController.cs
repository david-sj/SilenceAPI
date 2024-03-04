using Microsoft.AspNetCore.Mvc;
using SilenceAPI.Models;
using SilenceAPI.Services.Abtracts;

namespace SilenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailValidationController : ControllerBase
    {
        private readonly IEmailValidationService _emailValidationService;

        public EmailValidationController(IEmailValidationService emailValidationService)
        {
            _emailValidationService = emailValidationService;
        }

        [HttpPost("UploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName) != ".txt")
            {
                return BadRequest("Por favor, cargue un archivo en formato .txt");
            }

            EmailValidationCounter result = _emailValidationService.ValidateEmailsFromFile(file);
            return Ok(result);
        }

        [HttpGet("GetEmailValidationMetrics")]
        public IActionResult GetMetrics()
        {
            List<EmailValidationMetrics> result = _emailValidationService.GetMetrics();
            return Ok(result);
        }

        [HttpPost("SearchEmail")]
        public IActionResult SearchEmail(string searchText)
        {
            List<EmailData> result = _emailValidationService.SearchEmail(searchText);
            return Ok(result);
        }
    }
}
