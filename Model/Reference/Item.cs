namespace DocumentHelper.Model
{
    public class Item : Identifier
    {
        public string Name { get; set; } = string.Empty;

        public Unit? Unit { get; set; }

        public Section? Section { get; set; }
    }
}
