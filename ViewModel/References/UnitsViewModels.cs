using DocumentHelper.Model;
using DocumentHelper.ModelView.ReferencesVM;

namespace DocumentHelper.ModelView.References
{
    public partial class UnitsViewModel : ReferencesViewModel<Unit, UnitViewModel>
    {
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
    }
}
