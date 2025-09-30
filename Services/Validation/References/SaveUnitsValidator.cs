using DocumentHelper.Model;
using FluentValidation;

namespace DocumentHelper.Services.Validation
{
    class SaveUnitsValidator : AbstractValidator<List<Unit>>
    {
        public SaveUnitsValidator()
        {
            RuleForEach(doc => doc).ChildRules(pos =>
            {
                pos.RuleFor(x => x.FullName).NotEmpty().WithMessage("Полное название единицы измерения не заполнено!");
                pos.RuleFor(x => x.ShortName).NotEmpty().WithMessage("Краткое название единицы измерения не заполнено!");
            });
        }

    }
}
