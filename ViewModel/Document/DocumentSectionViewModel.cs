using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Services.Database;
using DocumentHelper.ViewModel.Commands;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace DocumentHelper.ModelView
{
    public partial class DocumentSectionViewModel
    {

        [Notify]
        private Section? _section = null!;

        private ObservableCollection<PositionViewModel> _positions = new();

        public PositionViewModel? SelectedPosition { get; set; }

        public event Action PositionTotalSumChanged;

        public string Name { get => Section.Name ?? "Без названия"; }

        public int Id
        {
            get => _section.Id;
            set
            {
                if (_section.Id != value)
                {
                    _section.Id = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }

        public ObservableCollection<PositionViewModel> Positions
        {
            get => _positions;
            set
            {
                if (_positions != value)
                {
                    if (_positions != null)
                    {
                        _positions.CollectionChanged -= OnPositionsCollectionChanged;
                        UnsubscribeFromPositionChanges(_positions);
                    }

                    _positions = value;

                    if (_positions != null)
                    {
                        _positions.CollectionChanged += OnPositionsCollectionChanged;
                        SubscribeToPositionChanges(_positions);
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Positions)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SectionTotalSum)));
                }
            }
        }

        public double SectionTotalSum => Positions.Sum(p => p.Position.TotalSum);

        public int PositionsCount => Positions.Count;

        public RelayCommand AddPositionCommand { get; private set; }
        public RelayCommand RemovePositionCommand { get; private set; }

        public DocumentSectionViewModel(Action handler)
        {
            InitializeCommands();
            Positions.CollectionChanged += OnPositionsCollectionChanged;
            SubscribeToPositionChanges(Positions);
            PositionTotalSumChanged += handler;
        }

        private void OnPositionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(SectionTotalSum)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(PositionsCount)));

            if (e.NewItems != null)
            {
                SubscribeToPositionChanges(e.NewItems.Cast<PositionViewModel>());
            }

            if (e.OldItems != null)
            {
                UnsubscribeFromPositionChanges(e.OldItems.Cast<PositionViewModel>());
            }
        }

        private void SubscribeToPositionChanges(IEnumerable<PositionViewModel> positions)
        {
            foreach (var position in positions)
            {
                position.PropertyChanged += OnPositionPropertyChanged;
            }
        }

        private void UnsubscribeFromPositionChanges(IEnumerable<PositionViewModel> positions)
        {
            foreach (var position in positions)
            {
                position.PropertyChanged -= OnPositionPropertyChanged;
            }
        }

        private void OnPositionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Position.TotalSum))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SectionTotalSum)));
                PositionTotalSumChanged();
            }
        }

        public void InitializeCommands()
        {
            AddPositionCommand = new RelayCommand(AddPosition);
            RemovePositionCommand = new RelayCommand(RemovePosition);
        }

        private void AddPosition(object parameter)
        {
            Positions.Add(new PositionViewModel() { Number = PositionsCount + 1, Section = _section });
        }

        private void RemovePosition(object parameter)
        {
            if (SelectedPosition is not null)
            {
                MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить данную позицию?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Positions.Remove(SelectedPosition); 
                    SelectedPosition = null;
                }
            }

            int counter = 1;
            foreach (var position in Positions)
            {
                position.PropertyChanged -= OnPositionPropertyChanged;
                position.Number = counter;
                counter++;
            }

            PositionTotalSumChanged();
        }
    }
}
