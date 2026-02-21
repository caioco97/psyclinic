namespace PsyClinic.Api.ViewModels.Auth
{
    public class RequestUserViewModel
    {
        public required string Email { get; init; }
        public required string Name { get; init; }
        public required string Password { get; init; }
        public required string Phone { get; init; }
        public required string FederalRegistration { get; set; }
    }
}
