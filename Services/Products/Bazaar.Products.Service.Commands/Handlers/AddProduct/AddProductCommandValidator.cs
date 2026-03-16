using FluentValidation;

namespace Bazaar.Products.Service.Commands.Handlers.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.Description).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.Category).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.Price).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.Currency).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.Unit).NotEmpty().WithMessage("The field shouldn't be empty");
            RuleFor(c => c.WeightPerUnitInKg).NotEmpty().WithMessage("The field shouldn't be empty");
        }
    }
}
