using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentHelper.Model
{
    public partial class Section : Identifier
    {

        public string Name { get; set; } = string.Empty;

        public int Number { get; set; }

        public string TotalDescription { get; set; } = string.Empty;

        public int TemplateTotalRow { get; set; }

        [NotMapped]
        public double TotalSum { get; set; }
    }
}
