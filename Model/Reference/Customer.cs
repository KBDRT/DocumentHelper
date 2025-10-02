using DocumentHelper.Model.Document;

namespace DocumentHelper.Model.Reference
{
    public class Customer : Identifier
    {
        public string Name { get; set; } = string.Empty;

        public DocumentDates DatePeriod { get; set; } = new();

        public string FullInfo => $"{Name}";

        public List<Address> Addresses { get; set; } = new();

        public List<Contract> Contracts { get; set; } = new();

    }
}
