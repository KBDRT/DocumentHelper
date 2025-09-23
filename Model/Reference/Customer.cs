using DocumentHelper.Model.Document;

namespace DocumentHelper.Model.Reference
{
    public class Customer : Identifier
    {
        public string Name { get; set; } = string.Empty;

        public Address Address { get; set; } = new();

        public DocumentDates DatePeriod { get; set; } = new();

    }
}
