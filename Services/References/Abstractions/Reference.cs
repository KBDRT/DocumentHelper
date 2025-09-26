using DocumentHelper.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DocumentHelper.Services.References.Abstractions
{
    public class Reference<Entity> : IGetReference where Entity : class
    {
        private List<Entity> _records { get; set; } = [];

        public void SelectReference()
        {
            using var context = new AppDBContext();
            var dbSet = context.Set<Entity>();
            _records = SelectFromDbSet(dbSet);
        }

        public IEnumerable GetRecords()
        {
            return _records;
        }

        public virtual List<Entity> SelectFromDbSet(DbSet<Entity> dbSet)
        {
            return dbSet.AsNoTracking().ToList();
        }

    }
}
