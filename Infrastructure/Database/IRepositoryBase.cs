using DocumentHelper.Model;
using DocumentHelper.Model.Document;

namespace DocumentHelper.Services.Database
{
    interface IRepositoryBase<TEntity> where TEntity : class
    {
        public int GetRecordsCount();

        public Task<List<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default);
        public Task SaveChangesWithTrackingAsync(IEnumerable<TEntity> updatedEntities);

        public Task SaveItems(List<Item> items);

        public Task SaveDocument(Document document);
    }
}
