using System.Collections.ObjectModel;

namespace DocumentHelper.Model.Document
{
    public class Document : Identifier
    {
        public int SystemNumber { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public DocumentDates DatePeriod { get; set; } = new();

        public ObservableCollection<Position> Positions { get; set; } = new();
    }
}
