using DocumentHelper.Model.Reference;
using DocumentHelper.Services.References.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.References.SpecificReferences
{
    public class AddressReference : Reference<Address>
    {
        public override List<Address> SelectFromDbSet(DbSet<Address> dbSet)
        {
            return dbSet.Include(x => x.Customer)
                .AsNoTracking().ToList();
        }
    }
}
