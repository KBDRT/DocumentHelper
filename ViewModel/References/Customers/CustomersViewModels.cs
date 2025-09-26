using DocumentHelper.Model;
using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView.ReferencesVM;

namespace DocumentHelper.ViewModel.References.Customers
{
    public partial class CustomersViewModel : ReferencesViewModel<Customer, CustomerViewModel>
    {
        public CustomersViewModel() : base()
        {
            ReferenceName = "Customer";
        }

        protected override Customer? CreateEntityFromViewModel(CustomerViewModel entityViewModel)
        {
            if (entityViewModel is CustomerViewModel customer)
            {
                return new Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    DatePeriod = new()
                    {
                        DateStart = customer.DateStart,
                        DateEnd = customer.DateEnd,
                    },
                    Address = new()
                    {
                        City = customer.City,
                        House = customer.House,
                        Street = customer.Street,
                    }
                };
            }

            return null;
        }

        protected override CustomerViewModel? CreateEntityViewModel(Identifier item)
        {
            if (item is Customer customer)
            {
                return new CustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    City = customer.Address.City,
                    Street = customer.Address.Street,
                    House = customer.Address.House,
                    DateStart = customer.DatePeriod.DateStart,
                    DateEnd = customer.DatePeriod.DateEnd,
                };
            }

            return null;
        }
    }
}
