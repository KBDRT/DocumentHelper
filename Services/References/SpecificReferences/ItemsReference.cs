using DocumentHelper.Model;
using DocumentHelper.Services.References.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.References.SpecificReferences
{
    public class ItemsReference : Reference<Item>
    {
        public override List<Item> SelectFromDbSet(DbSet<Item> dbSet)
        {
            return dbSet.Include(x => x.Section).Include(x => x.Unit).AsNoTracking().ToList();
        }
    }
}
