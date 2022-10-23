using FluentValidation;
using FooDash.Application.Notifications.Dtos.Contracts;

namespace FooDash.Application.Notifications.Dtos.Validators
{
    public class SendTestNotificationContractValidator : AbstractValidator<SendTestNotificationContract>
    {
        public SendTestNotificationContractValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}