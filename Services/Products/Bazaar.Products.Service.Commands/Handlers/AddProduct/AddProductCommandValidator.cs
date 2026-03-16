using FluentValidation;

namespace Bazaar.Products.Service.Commands.Handlers.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor.NotEmpty(c => c.Name).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.Description).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.Category).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.Price).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.Currency).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.Unit).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor.NotEmpty(c => c.WieghtPerUnitInKg).NotEmpty().WithMessage("The field shouldn't be empty");
        }
    }
}
