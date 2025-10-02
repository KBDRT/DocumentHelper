using DocumentHelper.Model;
using DocumentHelper.ModelView.ReferencesVM;
using DocumentHelper.Services.DataServices;

namespace DocumentHelper.ViewModel.References.Units
{
    public partial class UnitsViewModel : ReferencesViewModel<Unit, UnitViewModel>
    {
        public UnitsViewModel() : base()
        {
            ReferenceName = "Unit";
        }

        protected override Unit? CreateEntityFromViewModel(UnitViewModel entityViewModel)
        {
            if (entityViewModel is UnitViewModel unit)
            {
                return new Unit()
                {
                    Id = unit.Id,
                    FullName = unit.FullName,
                    ShortName = unit.ShortName,
                    Number = unit.Number
                };
            }

            return null;
        }

        protected override UnitViewModel? CreateEntityViewModel(Identifier item)
        {
            if (item is Unit unit)
            {
                return new UnitViewModel()
                {
                    Id = unit.Id,
                    FullName = unit.FullName,
                    Number = unit.Number,
                    ShortName = unit.ShortName
                };
            }

            return null;
        }

        protected override bool CheckDependent()
        {
            if (SelectedItem != null && ReferencesManager.GetReferenceByName("Item") is List<Item> items)
            {
                return !items.Any(x => x.UnitId == SelectedItem.Id);
            }

            return true;
        }
    }
}
