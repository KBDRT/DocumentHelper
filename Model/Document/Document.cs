using DocumentHelper.Model.Reference;
using System.Collections.ObjectModel;

namespace DocumentHelper.Model.Document
{
    public class Document : Identifier
    {
        public int SystemNumber { get; set; }

        public string Number { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public double Costs { get; set; }

        public double TotalSum { get => Positions.Sum(x => x.TotalSum) + Costs; } 

        public DateTime Date { get; set; } = DateTime.Now;

        public DocumentDates DatePeriod { get; set; } = new();

        public ObservableCollection<Position> Positions { get; set; } = new();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
