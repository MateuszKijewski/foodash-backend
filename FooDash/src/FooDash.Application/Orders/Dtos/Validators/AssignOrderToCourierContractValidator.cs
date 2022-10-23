using FluentValidation;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Orders.Dtos.Validators
{
    public class AssignOrderToCourierContractValidator : AbstractValidator<AssignOrderToCourierContract>
    {
        public AssignOrderToCourierContractValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.CourierId).NotEmpty();
        }
    }
}