using DocumentHelper.Model.Reference;
using FluentValidation;

namespace DocumentHelper.Services.Validation
{
    class SaveCustomersValidator : AbstractValidator<List<Customer>>
    {
        public SaveCustomersValidator()
        {
            RuleForEach(doc => doc).ChildRules(pos =>
            {
                pos.RuleFor(x => x.Name).NotEmpty().WithMessage("Пустое название заказчика!");
            });
        }
    }
}
