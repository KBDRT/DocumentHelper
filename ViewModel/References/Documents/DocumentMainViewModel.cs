using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ViewModel.References.Documents
{
    public partial class DocumentMainViewModel : BaseViewModel
    {
        private Document _document = new();

        public int Id { get => _document.Id; set => SetValueProperty(value, v => _document.Id = v); }

        public string Number { get => _document.Number; set => SetValueProperty(value, v => _document.Number = v); }

        public string Building { get => _document.Building; set => SetValueProperty(value, v => _document.Building = v); }

        private double _totalSum;

        public double TotalSum { get => _totalSum; set => SetValueProperty(value, v => _totalSum = v); }

        public DateTime Date { get => _document.Date; set => SetValueProperty(value, v => _document.Date = v); }

        public Customer Customer { get => _document.Customer; set => SetValueProperty(value, v => _document.Customer = v); }

        public string CustomerName { get => Customer?.Name; }

        public Address Address { get => _document.Address; set => SetValueProperty(value, v => _document.Address = v); }

        public string AddressFull { get => Address?.FullAddress; }

        public DocumentDates DatePeriod { get => _document?.DatePeriod; set => SetValueProperty(value, v => _document.DatePeriod = v); }

    }
}
