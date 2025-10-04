using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.ModelView.ReferencesVM;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DocumentHelper.ViewModel.References.Documents
{
    public partial class DocumentsViewModel : ReferencesViewModel<Document, DocumentMainViewModel>
    {
        private ICollectionView _documentsView = null!;

        public DocumentsFilter Filter { get; } = new();

        public DocumentsViewModel() : base()
        {
            ReferenceName = "Document";
            DocumentsView = CollectionViewSource.GetDefaultView(Reference);
            DocumentsView.Filter = Filter.Apply;
            ApplyFilterCommand = new(ApplyFilterCommandHandler);
            ClearFilterCommand = new(ClearFilterCommandHandler);
        }

        public ICollectionView DocumentsView
        {
            get => _documentsView;
            set
            {
                _documentsView = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Documents)));
            }
        }

        protected override Document? CreateEntityFromViewModel(DocumentMainViewModel entityViewModel)
        {
            return null;
        }

        protected override DocumentMainViewModel? CreateEntityViewModel(Identifier item)
        {
            if (item is Document doc)
            {
                return new DocumentMainViewModel()
                {
                    Id = doc.Id,
                    Customer = doc.Customer,
                    Address = doc.Address,
                    Date = doc.Date,
                    DatePeriod = doc.DatePeriod,
                    Number = doc.Number,
                    TotalSum = doc.TotalSum,
                    Building = doc.Building,
                };
            }

            return null;
        }

        protected override void ReferenceUpdated()
        {
            FillReference();
        }

        public RelayCommand ApplyFilterCommand { get; private set; } = null!;

        protected void ApplyFilterCommandHandler(object parameter) 
        {
            DocumentsView.Refresh();
        }

        public RelayCommand ClearFilterCommand { get; private set; } = null!;

        protected void ClearFilterCommandHandler(object parameter)
        {
            Filter.Clear();
            DocumentsView.Refresh();
        }

        protected async override void RemoveCommandHandler(object parameter)
        {
            if (SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                   "Вы уверены, что хотите удалить данный документ?",
                   "Подтверждение",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _repository.DeleteById(SelectedItem.Id);
                    Reference.Remove(SelectedItem);
                    SelectedItem = null;

                    ReferencesManager.SelectReferenceByName("Document");
                }
            }
        }

    }
}
