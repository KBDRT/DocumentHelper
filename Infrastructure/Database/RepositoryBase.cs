
using DocumentHelper.Infrastructure.Database;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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

                UpdateOwnedProperties(context, existingEntity, updatedEntity);
            }

            await context.SaveChangesAsync();
        }

        private void UpdateOwnedProperties(AppDBContext context, TEntity existing, TEntity updated)
        {
            var ownedNavigations = context.Model.FindEntityType(typeof(TEntity))
                ?.GetNavigations()
                ?.Where(n => n.TargetEntityType?.IsOwned() == true) ?? [];

            foreach (var navigation in ownedNavigations)
            {
                var updatedValue = typeof(TEntity).GetProperty(navigation.Name)?.GetValue(updated);
                var existingValue = typeof(TEntity).GetProperty(navigation.Name)?.GetValue(existing);

                if (updatedValue != null)
                {
                    if (existingValue == null)
                    {
                        // Создаем новый экземпляр owned-типа
                        existingValue = Activator.CreateInstance(updatedValue.GetType());
                        typeof(TEntity).GetProperty(navigation.Name)?.SetValue(existing, existingValue);
                    }

                    // Копируем значения
                    context.Entry(existingValue).CurrentValues.SetValues(updatedValue);
                }
            }
        }

        public async Task SaveItems(List<Item> items)
        {
            using var context = new AppDBContext();

            // Работаем только с ID, чтобы избежать проблем с отслеживанием
            var itemsToUpdate = items.Select(ui => new Item
            {
                Id = ui.Id,
                Name = ui.Name,
                UnitId = ui.Unit?.Id ?? ui.UnitId,
                SectionId = ui.Section?.Id ?? ui.SectionId,
            }).ToList();

            var existingItems = await context.Items.ToListAsync();

            // Добавление новых
            var newItems = itemsToUpdate.Where(ui => ui.Id == 0).ToList();
            await context.Items.AddRangeAsync(newItems);

            // Обновление существующих
            foreach (var existingItem in existingItems)
            {
                var updatedItem = itemsToUpdate.FirstOrDefault(ui => ui.Id == existingItem.Id);
                if (updatedItem != null)
                {
                    existingItem.Name = updatedItem.Name;
                    existingItem.UnitId = updatedItem.UnitId;
                }
            }

            // Удаление
            var toDelete = existingItems.Where(ei =>
                !itemsToUpdate.Any(ui => ui.Id == ei.Id));
            context.Items.RemoveRange(toDelete);

            await context.SaveChangesAsync();
        }

        private object GetKey(TEntity entity, AppDBContext context)
        {
            var keyName = context.Model.FindEntityType(typeof(TEntity))
                ?.FindPrimaryKey().Properties.Select(p => p.Name).First();

            return typeof(TEntity).GetProperty(keyName).GetValue(entity);
        }

        public async Task SaveDocument(Document document)
        {
            using var context = new AppDBContext();

            if (document.Id > 0)
            {
                await context.Documents
                          .Where(x => x.Id == document.Id)
                          .ExecuteDeleteAsync();

                document.Id = 0;

            }

            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();
        }
    }
}
