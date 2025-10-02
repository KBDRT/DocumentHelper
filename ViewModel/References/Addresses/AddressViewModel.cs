using DocumentHelper.Model.Reference;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ViewModel.References.Addresses
{
    public class AddressViewModel : BaseViewModel
    {
        private Address _address = new();

        public int Id { get => _address.Id; set => SetValueProperty(value, v => _address.Id = v); }

        public string FullAddress { get => _address.FullAddress; set => SetValueProperty(value, v => _address.FullAddress = v); }

        public Customer Customer { get => _address?.Customer; set => SetValueProperty(value, v => _address.Customer = v); }

        public Address GetAddress() => _address;
    }
}
