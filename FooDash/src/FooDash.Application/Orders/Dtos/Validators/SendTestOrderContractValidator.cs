using FluentValidation;
using FooDash.Application.Orders.Dtos.Contracts;

namespace FooDash.Application.Orders.Dtos.Validators
{
    public class SendTestOrderContractValidator : AbstractValidator<SendTestOrderContract>
    {
        public SendTestOrderContractValidator()
        {
            RuleFor(x => x.OrderNumber).NotEmpty();
        }
    }
}