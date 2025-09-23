using DocumentHelper.Commands;
using DocumentHelper.Model;
using DocumentHelper.Services;
using DocumentHelper.ViewModel.References.Abstractions;
using PropertyChanged.SourceGenerator;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DocumentHelper.ModelView.ReferencesVM
{
    public abstract partial class ReferencesViewModel<Entity, EntityViewModel> where Entity : Identifier 
                                                                               where EntityViewModel : class, new()
    {

        private readonly ReferenceService<Entity> _service = new();

        [Notify]
        protected ObservableCollection<EntityViewModel> _reference { get; set; } = new();

        public EntityViewModel? SelectedItem { get; set; }

        public RelayCommand AddCommand { get; private set; } = null!;
        public RelayCommand RemoveCommand { get; private set; } = null!;

        public RelayCommand SaveCommand { get; private set; } = null!;


        public ObservableCollection<EntityViewModel> Reference
        {
            get => _reference;
            set
            {
                if (_reference != value)
                {
                    _reference = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Reference)));
                }
            }
        }

        public ReferencesViewModel()
        {
            AddCommand = new RelayCommand(AddCommandHandler);
            RemoveCommand = new RelayCommand(RemoveCommandHandler);
            SaveCommand = new RelayCommand(SaveCommandHandler);

            foreach (var item in DataManager.GetReferenceByType(typeof(Entity)))
            {
                var newItem = CreateEntityViewModel(item);
                if (newItem != null)
                {
                    Reference.Add(newItem);
                }
            }
        }

        protected abstract EntityViewModel? CreateEntityViewModel(Identifier item);

        protected abstract Entity? CreateEntityFromViewModel(EntityViewModel entityViewModel);


        private void AddCommandHandler(object parameter)
        {
            Reference.Add(new EntityViewModel());
        }

        private void RemoveCommandHandler(object parameter)
        {
            if (SelectedItem is not null)
            {
                Reference.Remove(SelectedItem);
                SelectedItem = null;
            }
            else
                return;
        }

        private void SaveCommandHandler(object parameter)
        {
            var entityList = _reference.Select(uvm => CreateEntityFromViewModel(uvm)).ToList();
            if (entityList == null)
            {
                return;
            }

            _service.SaveReference(entityList);
            Reference.Clear();

            foreach (var item in entityList)
            {
                if (item != null)
                {
                    var entityItem = CreateEntityViewModel(item);
                    if (entityItem != null)
                    {
                        Reference.Add(entityItem);
                    }
                }
            }

        }
    }
}
