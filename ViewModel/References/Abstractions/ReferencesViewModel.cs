using DocumentHelper.Model;
using DocumentHelper.Services.Database;
using DocumentHelper.Services.DataServices;
using DocumentHelper.Services.Validation;
using DocumentHelper.ViewModel.Commands;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;

namespace DocumentHelper.ModelView.ReferencesVM
{
    public abstract partial class ReferencesViewModel<Entity, EntityViewModel> where Entity : Identifier 
                                                                               where EntityViewModel : class, new()
    {
        
        // репозиторий для работы с бд
        private readonly IRepositoryBase<Entity> _repository = new RepositoryBase<Entity>();

        // непосредственно справочник с данными
        [Notify]        
        protected ObservableCollection<EntityViewModel> _reference { get; set; } = new();

        // выбранный элемент
        public EntityViewModel? SelectedItem { get; set; }

        // команды
        public RelayCommand AddCommand { get; private set; } = null!;
        public RelayCommand RemoveCommand { get; private set; } = null!;

        public RelayCommand SaveCommand { get; private set; } = null!;

        // название справочника
        protected string ReferenceName { get; set; } = string.Empty;

        public ReferencesViewModel()
        {
            AddCommand = new RelayCommand(AddCommandHandler);
            RemoveCommand = new RelayCommand(RemoveCommandHandler);
            SaveCommand = new RelayCommand(SaveCommandHandler);
            FillReference();

            ReferencesManager.ReferenceUpdated += OnCurrentReferenceUpdated;
        }

        // необходимо реализовать для маппинга между viewmodel сущности и самой сущностью
        protected abstract EntityViewModel? CreateEntityViewModel(Identifier item);

        protected abstract Entity? CreateEntityFromViewModel(EntityViewModel entityViewModel);

        // логика заполнения справочника в viewmodel
        protected virtual void FillReference()
        {
            Reference.Clear();
            if (ReferencesManager.GetReferenceByType<Entity>() is List<Entity> items)
            {
                foreach (var item in items)
                {
                    var newItem = CreateEntityViewModel(item);
                    if (newItem != null)
                    {
                        Reference.Add(newItem);
                    }
                }
            }
        }

        // можно отслеживать изменение в бд справочника
        private void OnCurrentReferenceUpdated(object sender, string referenceName)
        {
            if (referenceName == ReferenceName)
            {
                ReferenceUpdated();
            }
        }

        // действие при обновлении справоничка в бд
        protected virtual void ReferenceUpdated() { }

        // реализация комманд
        protected virtual void AddCommandHandler(object parameter)
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

        private async void SaveCommandHandler(object parameter)
        {
            var entityList = _reference.Select(uvm => CreateEntityFromViewModel(uvm)).ToList();
            if (entityList == null)
            {
                return;
            }

            try
            {
                if (!ValidatorsService.ValidateWithWindow(entityList))
                    return;
            }
            catch
            {

            }


            await _repository.SaveChangesWithTrackingAsync(entityList);
            ReferencesManager.SelectReferenceByName(ReferenceName);

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
