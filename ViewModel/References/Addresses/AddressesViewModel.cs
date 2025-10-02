using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView;
using DocumentHelper.ModelView.ReferencesVM;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DocumentHelper.ViewModel.References.Addresses
{
    public class AddressesViewModel : ReferencesViewModel<Address, AddressViewModel>
    {
        private ICollectionView _addressesView = null!;

        private Customer _customer = null!;

        private DocumentViewModel? _document;

        private Window? _window;

        public Visibility IsVisibleSelectButton 
        { 
            get
            {
                return _document == null ? Visibility.Collapsed : Visibility.Visible;   
            }
        }

        public AddressesViewModel(Customer customer, DocumentViewModel? document = null, Window? window = null) : base()
        {
            ReferenceName = "Address";

            if (ReferencesManager.GetReferenceByName("Customer") is List<Customer> customerReference)
            {
                _customer = customerReference.SingleOrDefault(x => x.Id == customer.Id) ?? customer;
            }
            else
            {
                _customer = customer;
            }

            _document = document;
            _window = window;
            AddressesView = CollectionViewSource.GetDefaultView(_reference);
            AddressesView.Filter = Apply;
            SelectCommand = new(SelectCommandHandler);
        }

        public RelayCommand SelectCommand { get; private set; } = null!;

        private void SelectCommandHandler(object parameter)
        {
            if (_document != null && SelectedItem != null)
            {
                if (SelectedItem.Id == 0)
                {
                    MessageBox.Show("Необходимо сохранить справочник перед выбором", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _document.Address = SelectedItem.GetAddress();
                _window?.Close();
            }
        }

        protected override void AddCommandHandler(object parameter)
        {
            Reference.Add(new AddressViewModel() {  Customer = _customer});
        }

        public ICollectionView AddressesView
        {
            get => _addressesView;
            set
            {
                _addressesView = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AddressesView)));
            }
        }

        public Customer Customer
        {
            get => _customer;
        }

        protected override Address? CreateEntityFromViewModel(AddressViewModel entityViewModel)
        {
            if (entityViewModel is AddressViewModel address)
            {
                return new Address()
                {
                    Id = address.Id,
                    FullAddress = address.FullAddress,
                    Customer = address.Customer,
                    CustomerId = address.Customer.Id,
                };
            }

            return null;
        }

        protected override AddressViewModel? CreateEntityViewModel(Identifier item)
        {
            if (item is Address address)
            {
                return new AddressViewModel()
                {
                    Id = address.Id,
                    FullAddress = address.FullAddress,
                    Customer = address?.Customer,
                };
            }

            return null;
        }

        protected override bool CheckDependent()
        {
            if (SelectedItem != null && ReferencesManager.GetReferenceByName("Document") is List<Document> documents)
            {
                return !documents.Any(x => x.AddressId == SelectedItem.Id);
            }

            return true;
        }

        public bool Apply(object obj)
        {
            if (obj is not AddressViewModel address)
            {
                return true;
            }

            if (address.Customer == null || address.Customer.Id != _customer.Id)
            {
                return false;
            }

            return true;
        }

        protected override void ActionBeforeSave(List<Address?> entityList)
        {
            foreach (var entity in entityList)
            {
                entity.CustomerId = entity.Customer.Id;
                entity.Customer = null;
            }
        }


    }
}
