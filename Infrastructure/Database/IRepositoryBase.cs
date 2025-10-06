using DocumentHelper.Model;
using DocumentHelper.Model.Document;

namespace DocumentHelper.Services.Database
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        public int GetRecordsCount();

        public Task DeleteById(int entityId);

        public Task<List<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default);
        public Task SaveChangesWithTrackingAsync(IEnumerable<TEntity> updatedEntities);

        public Task SaveItems(List<Item> items);

        public Task SaveUnits(List<Unit> units);

        public Task SaveDocument(Document document);
    }
}
