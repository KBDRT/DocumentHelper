namespace DocumentHelper.Model
{
    public class Unit : Identifier
    {
        public int Number { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public int Code { get; set; } = 0;
    }
}
