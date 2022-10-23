using FluentValidation;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Orders.Dtos.Validators
{
    public class CreateOrderContractValidator : AbstractValidator<CreateOrderContract>
    {
        public CreateOrderContractValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.AddressLine1).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
            RuleFor(x => x.Longitude).NotEmpty();
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
        }
    }
}