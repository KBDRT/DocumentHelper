using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.Validation.References;
using DocumentHelper.View.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace DocumentHelper.Services.Validation
{
    class ValidatorsService
    {
        private static readonly Dictionary<Type, Type> _validators = new Dictionary<Type, Type>();

        public static void Register<TModel, TValidator>()
            where TValidator : IValidator<TModel>
        {
            _validators[typeof(TModel)] = typeof(TValidator);
        }


        public static IValidator<TModel> Create<TModel>()
        {
            if (_validators.TryGetValue(typeof(TModel), out Type validatorType))
            {
                return (IValidator<TModel>)Activator.CreateInstance(validatorType);
            }

            throw new InvalidOperationException($"Validator for type {typeof(TModel).Name} is not registered");
        }

        static ValidatorsService()
        {
            Register<Document, SaveDocumentValidator>();
            Register<List<Customer>, SaveCustomersValidator>();
            Register<List<Unit>, SaveUnitsValidator>();
            Register<List<Item>, SaveItemsValidator>();
        }

        public static ValidationResult Validate<TModel>(TModel model)
        {
            var validator = Create<TModel>();
            return validator.Validate(model);
        }

        public static bool ValidateWithWindow<TModel>(TModel model)
        {
            var validator = Create<TModel>();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var errorWindow = new ErrorWindow(validationResult.Errors);
                errorWindow.ShowDialog();
            }

            return validationResult.IsValid;
        }
    }
}
