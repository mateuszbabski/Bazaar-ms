using Bazaar.Customers.Service.Commands.Handlers.Update;
using FluentValidation;

namespace Bazaar.Customers.Service.Commands.Handlers.UpdateToShop
{
    public class UpdateToShopValidator : AbstractValidator<UpdateToShopCommand>
    {
        public UpdateToShopValidator()
        {
            RuleFor(c => c.ShopName).NotEmpty().WithMessage("The field shouldn't be empty");
        }
    }
}
