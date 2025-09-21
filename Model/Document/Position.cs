namespace DocumentHelper.Model.Document
{
    public class Position : Identifier
    {
        public int Number;

        public Item? Item;

        public double Amount;

        public double Price;

        public double TotalSum => Amount * Price;

        public Document? Document;

        public Section? Section;
    }
}
