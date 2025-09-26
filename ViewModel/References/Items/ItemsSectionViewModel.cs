using DocumentHelper.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PropertyChanged.SourceGenerator;
using DocumentHelper.ViewModel.Commands;

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
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
