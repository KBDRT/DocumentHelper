
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.References.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.References.SpecificReferences
{
    public class ContractReference : Reference<Contract>
    {
        public override List<Contract> SelectFromDbSet(DbSet<Contract> dbSet)
        {
            return dbSet.Include(x => x.Customer)
                .AsNoTracking().ToList();
        }
    }
}
