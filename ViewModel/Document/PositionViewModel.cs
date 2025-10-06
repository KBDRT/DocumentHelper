using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.View.References;
using DocumentHelper.ViewModel.References.Units;
using PropertyChanged.SourceGenerator;
using System.ComponentModel;

namespace DocumentHelper.ModelView
{
    public partial class PositionViewModel
    {
        [Notify]
        private Position position = new() { Item = new(), Unit = new() };

        private ItemViewModel _item = new();

        private Unit _unit = new();

        public int Id
        {
            get => position.Id;
            set
            {
                if (position.Id != value)
                {
                    position.Id = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }

        public int Number
        {
            get => position.Number;
            set
            {
                if (position.Number != value)
                {
                    position.Number = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }

        public double TotalSum
        {
            get => position.TotalSum;
        }

        public double Amount
        {
            get => position.Amount;
            set
            {
                if (position.Amount != value)
                {
                    position.Amount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Amount)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalSum)));
                }
            }
        }

        public double Price
        {
            get => position.Price;
            set
            {
                if (position.Price != value)
                {
                    position.Price = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Price)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalSum)));
                }
            }
        }

        public Section Section
        {
            get => position?.Section;
            set
            {
                if (position.Section != value)
                {
                    position.Section = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Section)));
                }
            }
        }

        public ItemViewModel Item
        {
            get => _item;
            set
            {
                if (_item != value)
                {
                    _item = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Item)));
                }
            }
        }

        public Unit Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Unit)));
                }
            }
        }



    }
}
