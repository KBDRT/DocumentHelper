using DocumentHelper.Model.Document;
using PropertyChanged.SourceGenerator;
using System.ComponentModel;

namespace DocumentHelper.ModelView
{
    public partial class PositionViewModel
    {
        [Notify]
        private Position position = new();

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

    }
}
