namespace DocumentHelper.Model.Reference
{

    public class Address : Identifier
    {
        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string House { get; set; } = string.Empty;

        public string FullAddress {  get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

    }
}
