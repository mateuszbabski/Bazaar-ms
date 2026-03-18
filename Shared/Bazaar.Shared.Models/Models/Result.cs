namespace Bazaar.Shared.Models.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
