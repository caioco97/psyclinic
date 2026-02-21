namespace PsyClinic.Api.ViewModels.Auth
{
    public class ValidationErrorResponse
    {
        public bool Success => false;
        public List<string> Errors { get; set; } = new();
    }
}
