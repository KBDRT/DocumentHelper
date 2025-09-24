using System.Collections.ObjectModel;

namespace DocumentHelper.Model.Document
{
    public class Document : Identifier
    {
        public int SystemNumber { get; set; }

        public int Number { get; set; }

        public double Costs { get; set; }

        public double TotalSum { get => Positions.Sum(x => x.TotalSum); } 

        public DateTime Date { get; set; } = DateTime.Now;

        public DocumentDates DatePeriod { get; set; } = new();

        public ObservableCollection<Position> Positions { get; set; } = new();
    }
}
