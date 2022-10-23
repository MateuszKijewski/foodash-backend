namespace FooDash.Application.Auth.Dtos.Responses
{
    public class RegisterUserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}