
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.References.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.References.SpecificReferences
{
    public class CustomerReference : Reference<Customer>
    {
        public override List<Customer> SelectFromDbSet(DbSet<Customer> dbSet)
        {
            return dbSet.Include(x => x.Addresses)
                .Include(x => x.Contracts)
                .AsNoTracking().ToList();
        }
    }
}
