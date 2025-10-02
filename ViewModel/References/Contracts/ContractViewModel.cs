using DocumentHelper.Model.Reference;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ViewModel.References.Contracts
{
    public class ContractViewModel : BaseViewModel
    {
        private Contract _contract = new();

        public int Id { get => _contract.Id; set => SetValueProperty(value, v => _contract.Id = v); }

        public string Number { get => _contract.Number; set => SetValueProperty(value, v => _contract.Number = v); }

        public DateTime Date { get => _contract.Date; set => SetValueProperty(value, v => _contract.Date = v); }

        public string Type { get => _contract.Type; set => SetValueProperty(value, v => _contract.Type = v); }

        public Customer Customer { get => _contract?.Customer; set => SetValueProperty(value, v => _contract.Customer = v); }

        public Contract GetContract() => _contract;
    }
}
