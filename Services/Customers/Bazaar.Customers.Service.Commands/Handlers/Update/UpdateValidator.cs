using FluentValidation;

namespace Bazaar.Customers.Service.Commands.Handlers.Update
{
    public class UpdateValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateValidator()
        {
            RuleFor(c => c.Phone).Matches("^[0-9 +-]+$")
                                 .WithMessage("Only digits allowed");
        }
    }
}
