using DocumentHelper.Model.Reference;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ModelView.References
{
    public partial class CustomerViewModel : BaseViewModel
    {
        private Customer _customer = new();

        public int Id { get => _customer.Id; set => SetValueProperty(value, v => _customer.Id = v); }

        public string Name { get => _customer.Name; set => SetValueProperty(value, v => _customer.Name = v); }

        public DateTime DateStart { get => _customer.DatePeriod.DateStart; set => SetValueProperty(value, v => _customer.DatePeriod.DateStart = v); }

        public DateTime DateEnd { get => _customer.DatePeriod.DateEnd; set => SetValueProperty(value, v => _customer.DatePeriod.DateEnd = v); }

        public string City { get => _customer.Address.City; set => SetValueProperty(value, v => _customer.Address.City = v); }

        public string House { get => _customer.Address.House; set => SetValueProperty(value, v => _customer.Address.House = v); }

        public string Street { get => _customer.Address.Street; set => SetValueProperty(value, v => _customer.Address.Street = v); }

    }
}
