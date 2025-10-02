using DocumentHelper.Model.Reference;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ViewModel.References.Customers
{
    public partial class CustomerViewModel : BaseViewModel
    {
        private Customer _customer = new();

        public int Id { get => _customer.Id; set => SetValueProperty(value, v => _customer.Id = v); }

        public string Name { get => _customer.Name; set => SetValueProperty(value, v => _customer.Name = v); }

        public DateTime DateStart { get => _customer.DatePeriod.DateStart; set => SetValueProperty(value, v => _customer.DatePeriod.DateStart = v); }

        public DateTime DateEnd { get => _customer.DatePeriod.DateEnd; set => SetValueProperty(value, v => _customer.DatePeriod.DateEnd = v); }


        public Customer GetCustomer => _customer;

    }
}
