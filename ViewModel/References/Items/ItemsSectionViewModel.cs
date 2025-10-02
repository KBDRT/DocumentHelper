using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.Commands;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DocumentHelper.ModelView
{
    public partial class ItemsSectionViewModel
    {
        [Notify]
        private Section? _section;

        private ObservableCollection<ItemViewModel> _items = new();

        public ItemViewModel? SelectedItem { get; set; }

        public string Name { get => Section?.Name ?? "Без названия"; }

        public ObservableCollection<ItemViewModel> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    //if (_items != null)
                    //{
                    //    _items.CollectionChanged -= OnPositionsCollectionChanged;
                    //    UnsubscribeFromPositionChanges(_items);
                    //}

                    _items = value;

                    //if (_items != null)
                    //{
                    //    _items.CollectionChanged += OnPositionsCollectionChanged;
                    //    SubscribeToPositionChanges(_items);
                    //}

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
                }
            }
        }

        public RelayCommand AddCommand { get; private set; } = null!;
        public RelayCommand RemoveCommand { get; private set; } = null!;

        public ItemsSectionViewModel()
        {
            InitializeCommands();
        }


        public void InitializeCommands()
        {
            AddCommand = new RelayCommand(AddCommandHandler);
            RemoveCommand = new RelayCommand(RemoveCommandHandler);
        }

        private void AddCommandHandler(object parameter)
        {
            Items.Add(new ItemViewModel() { Section = _section});
        }

        private void RemoveCommandHandler(object parameter)
        {
            if (SelectedItem is not null)
            {
                if (CheckDependent())
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                }
                else
                {
                    MessageBox.Show("Удаление невозможно, есть зависимые данные!",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Stop);
                } 
            }
        }

        private bool CheckDependent()
        {
            if (SelectedItem != null && ReferencesManager.GetReferenceByName("Document") is List<Document> documents)
            {
                return !documents.Any(x => x.Positions.Any(x => x.ItemId == SelectedItem.Id));
            }

            return true;
        }
    }
}
