using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Model.Document
{
    [Owned]
    public class DocumentDates
    {
        public DateTime DateStart { get; set; } = DateTime.MinValue;

        public DateTime DateEnd { get; set; } = DateTime.MaxValue;
    }
}
