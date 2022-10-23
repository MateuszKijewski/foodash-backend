namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class CreateOrderContract
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string? Notes { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Guid CartId { get; set; }
        public Guid? OrderingUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}