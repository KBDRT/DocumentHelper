namespace DocumentHelper.Model
{
    public class Item : Identifier
    {
        public string Name { get; set; } = string.Empty;

        public int UnitId { get; set; }

        public Unit? Unit { get; set; }

        public int SectionId { get; set; }

        public Section? Section { get; set; }
    }
}
