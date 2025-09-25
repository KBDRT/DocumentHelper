using DocumentHelper.Model;
using DocumentHelper.ViewModel.References.Abstractions;

namespace DocumentHelper.ModelView
{
    public partial class ItemViewModel : BaseViewModel
    {
        private Item _item = new();

        public int Id { get => _item.Id; set => SetValueProperty(value, v => _item.Id = v); }

        public string Name { get => _item.Name; set => SetValueProperty(value, v => _item.Name = v); }

        public Unit? Unit { get => _item.Unit; set => SetValueProperty(value, v => _item.Unit = v); }

        public Section? Section { get => _item.Section; set => SetValueProperty(value, v => _item.Section = v); }

        public Item GetItem()
        {
            return _item;
        }
    }
}
