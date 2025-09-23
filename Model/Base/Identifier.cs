using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentHelper.Model
{
    public class Identifier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
