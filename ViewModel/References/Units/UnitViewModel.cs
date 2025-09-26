using DocumentHelper.Model;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ViewModel.References.Units
{
    public partial class UnitViewModel : BaseViewModel
    {
        private Unit _unit = new();

        public int Id { get => _unit.Id; set => SetValueProperty(value, v => _unit.Id = v); }

        public string FullName { get => _unit.FullName; set => SetValueProperty(value, v => _unit.FullName = v); }

        public string ShortName { get => _unit.ShortName; set => SetValueProperty(value, v => _unit.ShortName = v); }

        public int Number { get => _unit.Number; set => SetValueProperty(value, v => _unit.Number = v); }

    }
}
