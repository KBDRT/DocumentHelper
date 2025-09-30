

using DocumentHelper.Model;
using FluentValidation;

namespace DocumentHelper.Services.Validation.References
{
    class SaveItemsValidator : AbstractValidator<List<Item>>
    {
        public SaveItemsValidator() 
        {
            RuleForEach(item => item).ChildRules(pos =>
            {
                pos.RuleFor(x => x.Name).NotEmpty().WithMessage("Название услуги или материала не заполнено!");
                //pos.RuleFor(x => x.Unit).NotNull().WithMessage("Не выбрана единица измерения!");
                pos.RuleFor(x => x.Unit).NotNull().WithMessage("Не выбрана единица измерения!");
            });
        }
    }
}
