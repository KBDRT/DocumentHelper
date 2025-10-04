namespace DocumentHelper.Model.Document
{
    public class Position : Identifier
    {
        public int Number { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public double Amount { get; set; }

        public double Price { get; set; }

        public double TotalSum => Amount * Price;

        public int DocumentId { get; set; }
        public Document? Document { get; set; }

        public int SectionId { get; set; }
        public Section? Section { get; set; }

        public int UnitId { get; set; }

        public Unit? Unit { get; set; }
    }
}
