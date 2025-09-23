
using DocumentHelper.Infrastructure.Database;
using DocumentHelper.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services.Database
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Identifier
    {

        public async Task<List<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var context = new AppDBContext();
            return await context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public int GetRecordsCount()
        {
            using var context = new AppDBContext();
            return context.Set<TEntity>().AsNoTracking().Count();
        }

        public async Task SaveChangesWithTrackingAsync(IEnumerable<TEntity> updatedEntities)
        {
            using var context = new AppDBContext();
            // Получаем существующие entities из базы
            var existingEntities = await context.Set<TEntity>().ToListAsync();

            // Находим entities для удаления (есть в базе, но нет в updatedEntities)
            var entitiesToDelete = existingEntities
                .Where(e => !updatedEntities.Any(ue => GetKey(ue, context).Equals(GetKey(e, context))))
                .ToList();

            // Находим entities для добавления (есть в updatedEntities, но нет в базе)
            var entitiesToAdd = updatedEntities
                .Where(ue => !existingEntities.Any(e => GetKey(e, context).Equals(GetKey(ue, context))))
                .ToList();

            // Находим entities для обновления (есть в обоих списках)
            var entitiesToUpdate = updatedEntities
                .Where(ue => existingEntities.Any(e => GetKey(e, context).Equals(GetKey(ue, context))))
                .ToList();

            // Удаляем
            context.Set<TEntity>().RemoveRange(entitiesToDelete);

            // Добавляем новые
            await context.Set<TEntity>().AddRangeAsync(entitiesToAdd);

            // Обновляем существующие
            foreach (var updatedEntity in entitiesToUpdate)
            {
                var existingEntity = existingEntities
                    .First(e => GetKey(e, context).Equals(GetKey(updatedEntity, context)));

                context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            }

            await context.SaveChangesAsync();
        }

        private object GetKey(TEntity entity, AppDBContext context)
        {
            var keyName = context.Model.FindEntityType(typeof(TEntity))
                .FindPrimaryKey().Properties.Select(p => p.Name).First();

            return typeof(TEntity).GetProperty(keyName).GetValue(entity);
        }
    }
}
