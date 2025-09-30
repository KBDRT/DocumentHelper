using DocumentHelper.Model.Document;
using FluentValidation;

namespace DocumentHelper.Services.Validation
{
    public class SaveDocumentValidator : AbstractValidator<Document>
    {
        public SaveDocumentValidator()
        {
            RuleFor(doc => doc.Number).NotEmpty().WithMessage("Номер документа не может быть пустой!");

            RuleFor(doc => doc.CustomerId).NotEqual(0).WithMessage("Не задан заказчик!");

            RuleForEach(doc => doc.Positions).ChildRules(pos =>
            {
                pos.RuleFor(x => x.Item.UnitId).NotEqual(0).When(x => x.Item != null).WithMessage("У позиции не выбрана единица измерения!");
                pos.RuleFor(x => x.Item.Name).NotEmpty().When(x => x.Item != null).WithMessage("У позиции пустое название услуги или материала!");
            });
        }
    }
}
