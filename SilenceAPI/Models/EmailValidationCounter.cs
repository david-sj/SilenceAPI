namespace SilenceAPI.Models
{
    public class EmailValidationCounter
    {
        public Guid LoadId { get; set; }
        public int ValidEmailsCount { get; set; }
        public int InvalidEmailsCount { get; set; }
    }
}
