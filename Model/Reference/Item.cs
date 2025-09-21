namespace DocumentHelper.Model
{
    public class Item : Identifier
    {
        public string Name = string.Empty;

        public Unit? Unit;

        public Section? Section;
    }
}
