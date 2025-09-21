namespace DocumentHelper.Model.Reference
{
    public class Customer : Identifier
    {
        public string Name = string.Empty;

        public Address? Address;

        public DateOnly DateStart;

        public DateOnly DateEnd;
        
    }
}
