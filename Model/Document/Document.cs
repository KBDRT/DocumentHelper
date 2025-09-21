using System.Collections.ObjectModel;

namespace DocumentHelper.Model.Document
{
    public class Document : Identifier
    {
        public int SystemNumber;

        public int Number;

        public DateTime Date = DateTime.Now;

        public DocumentDates DatePeriod = new();

        public ObservableCollection<Position> Positions = new();
    }
}
