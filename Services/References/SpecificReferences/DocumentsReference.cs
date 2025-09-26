using DocumentHelper.Model.Document;
using DocumentHelper.Services.References.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.References.SpecificReferences
{
    public class DocumentsReference : Reference<Document>
    {
        public override List<Document> SelectFromDbSet(DbSet<Document> dbSet)
        {
            return dbSet.Include(x => x.Positions)
                .ThenInclude(x => x.Item)
                .ThenInclude(x => x.Unit)
                .Include(x => x.Positions)
                .ThenInclude(x => x.Section)
                .AsNoTracking().ToList();
        }
    }
}
