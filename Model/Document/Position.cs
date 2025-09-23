namespace DocumentHelper.Model.Document
{
    public class Position : Identifier
    {
        public int Number { get; set; }

        public Item? Item { get; set; }

        public double Amount { get; set; }

        public double Price { get; set; }

        public double TotalSum => Amount * Price;

        public Document? Document { get; set; }

        public Section? Section { get; set; }
    }
}
