using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView;
using DocumentHelper.ModelView.ReferencesVM;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.Commands;
using DocumentHelper.ViewModel.References.Addresses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DocumentHelper.ViewModel.References.Contracts
{
    public class ContractsViewModel : ReferencesViewModel<Contract, ContractViewModel>
    {
        private ICollectionView _contractView = null!;

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


        public ContractsViewModel(Customer customer, DocumentViewModel? document = null, Window? window = null) : base()
        {
            ReferenceName = "Contract";

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
            ContractsView = CollectionViewSource.GetDefaultView(_reference);
            ContractsView.Filter = Apply;
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
                _document.Contract = SelectedItem.GetContract();
                _window?.Close();
            }
        }

        protected override void AddCommandHandler(object parameter)
        {
            Reference.Add(new ContractViewModel() { Customer = _customer });
        }

        public ICollectionView ContractsView
        {
            get => _contractView;
            set
            {
                _contractView = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ContractsView)));
            }
        }

        public Customer Customer
        {
            get => _customer;
        }

        protected override Contract? CreateEntityFromViewModel(ContractViewModel entityViewModel)
        {
            if (entityViewModel is ContractViewModel contract)
            {
                return new Contract()
                {
                    Id = contract.Id,
                    Date = contract.Date,
                    Number = contract.Number,
                    Type = contract.Type,
                    Customer = contract.Customer,
                    CustomerId = contract.Customer.Id,
                };
            }

            return null;
        }

        protected override ContractViewModel? CreateEntityViewModel(Identifier item)
        {
            if (item is Contract contract)
            {
                return new ContractViewModel()
                {
                    Id = contract.Id,
                    Date = contract.Date,
                    Number = contract.Number,
                    Type = contract.Type,
                    Customer = contract?.Customer
                };
            }

            return null;
        }

        protected override bool CheckDependent()
        {
            if (SelectedItem != null && ReferencesManager.GetReferenceByName("Document") is List<Document> documents)
            {
                return !documents.Any(x => x.ContractId == SelectedItem.Id);
            }

            return true;
        }

        public bool Apply(object obj)
        {
            if (obj is not ContractViewModel contract)
            {
                return true;
            }

            if (contract.Customer == null || contract.Customer.Id != _customer.Id)
            {
                return false;
            }

            return true;
        }

        protected override void ActionBeforeSave(List<Contract?> entityList)
        {
            foreach (var entity in entityList)
            {
                entity.CustomerId = entity.Customer.Id;
                entity.Customer = null;
            }
        }
    }
}
