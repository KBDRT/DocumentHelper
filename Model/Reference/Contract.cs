
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentHelper.Model.Reference
{
    public class Contract : Identifier
    {
        public string Number { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public string Type { get; set; } = string.Empty;    

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [NotMapped]
        public string Info => $"{Number}, {Date.ToString("d")}";

    }
}
