using DocumentHelper.Model;
using DocumentHelper.Services;
using DocumentHelper.Services.Database;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DocumentHelper.ModelView
{
    public partial class ItemsViewModel
    {
        private readonly IRepositoryBase<Item> _repository = new RepositoryBase<Item>();

        public ObservableCollection<ItemsSectionViewModel> Sections { get; set; } = new();

        public ObservableCollection<Unit> AvailableUnits { get; set; } = new();

        public RelayCommand SaveCommand { get; private set; } = null!;
        public ItemsViewModel()
        {
            if (ReferencesManager.GetReferenceByName("Unit") is List<Unit> unitsReference)
            {
                foreach (var unit in unitsReference)
                {
                    AvailableUnits.Add(unit);
                }
            }

            var items = ReferencesManager.GetReferenceByName("Item") as List<Item>;
            var sections = ReferencesManager.GetReferenceByName("Section") as List<Section>;
            foreach (var section in sections)
            {
                var itemsForSection = new ObservableCollection<ItemViewModel>();
                foreach (var item in items.Where(x => x.SectionId == section.Id))
                {
                    itemsForSection.Add(new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Section = section,
                        Unit = AvailableUnits.SingleOrDefault(x=> x.Id == item.UnitId)
                    });
                }

                ItemsSectionViewModel newSection = new() { Section = section, Items = itemsForSection };
                Sections.Add(newSection);
                newSection.InitializeCommands();
            }

            SaveCommand = new(SaveCommandHandler);
        }

        private async void SaveCommandHandler(object parameter)
        {
            List<Item> items = new();
            foreach (var section in Sections)
            {
                foreach (var item in section.Items)
                {
                    items.Add(item.GetItem());
                }
            }

            if (items.Any(x => x.Unit == null))
            {
                MessageBox.Show("Ошибка сохранения!\nНе у всех позиций указана единица измерения!", 
                    "Ошибка", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                );

                return;
            }

            await _repository.SaveItems(items);
            ReferencesManager.SelectReferenceByName("Item");
        }
    }
}
