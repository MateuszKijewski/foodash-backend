using FluentValidation;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Orders.Dtos.Validators
{
    public class ChangeOrderStatusContractValidator : AbstractValidator<ChangeOrderStatusContract>
    {
        public ChangeOrderStatusContractValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.OrderStatus).NotEmpty();
        }
    }
}