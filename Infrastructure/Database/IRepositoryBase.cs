namespace DocumentHelper.Services.Database
{
    interface IRepositoryBase<TEntity> where TEntity : class
    {
        public int GetRecordsCount();

        public Task<List<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default);
        public Task SaveChangesWithTrackingAsync(IEnumerable<TEntity> updatedEntities);
    }
}
