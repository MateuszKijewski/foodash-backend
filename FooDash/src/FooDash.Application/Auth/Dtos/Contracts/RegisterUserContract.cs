namespace FooDash.Application.Auth.Dtos.Contracts
{
    public class RegisterUserContract
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}