using DocumentHelper.Commands;
using DocumentHelper.Model;
using DocumentHelper.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DocumentHelper.ModelView
{
    public partial class ItemsViewModel
    {
        private readonly ReferenceService<Item> _reference = new();

        public ObservableCollection<ItemsSectionViewModel> Sections { get; set; } = new();

        public ObservableCollection<Unit> AvailableUnits { get; set; } = new();

        public RelayCommand SaveCommand { get; private set; } = null!;
        public ItemsViewModel()
        {
            if (DataManager.GetReferenceByType(typeof(Unit)) is List<Unit> unitsReference)
            {
                foreach (var unit in unitsReference)
                {
                    AvailableUnits.Add(unit);
                }
            }

            var items = DataManager.GetReferenceByType(typeof(Item)) as List<Item>;
            foreach (var section in DataManager.Sections)
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

        private void SaveCommandHandler(object parameter)
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

            _reference.SaveItems(items);
        }
    }
}
