using DocumentHelper.Model;
using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView.ReferencesVM;
using DocumentHelper.ViewModel.References.Abstractions;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;

namespace DocumentHelper.ModelView.References
{
    public partial class CustomersViewModel : ReferencesViewModel<Customer, CustomerViewModel>
    {
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
