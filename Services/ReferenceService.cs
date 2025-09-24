using DocumentHelper.Model;
using DocumentHelper.Services.Database;
using System.Collections.ObjectModel;

namespace DocumentHelper.Services
{
    public class ReferenceService<Entity> where Entity : Identifier
    {
        private readonly IRepositoryBase<Entity> _repository;

        public ReferenceService()
        {
            _repository = new RepositoryBase<Entity>();
        }

        public async void SaveReference(IEnumerable<Entity> referenceList, string referenceName)
        {
            await _repository.SaveChangesWithTrackingAsync(referenceList);
            DataManager.ReselectReferenceByName(referenceName);
        }

        public async void SaveItems(List<Item> items)
        {
            await _repository.SaveItems(items);
            DataManager.ReselectReferenceByName("Item");
        }

    }
}
