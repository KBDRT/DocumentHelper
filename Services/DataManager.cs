using DocumentHelper.Infrastructure.Database;
using DocumentHelper.Model;
using DocumentHelper.Model.Reference;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Services
{
    public static class DataManager
    {
        public static List<Section> Sections = new();

        private static Dictionary<Type, IEnumerable<Identifier>> _references = new Dictionary<Type, IEnumerable<Identifier>>
        {
           { typeof(Unit),          new List<Unit>() },
           { typeof(Customer),      new List<Customer>() },
           { typeof(Item),          new List<Item>() },
        };

        static DataManager()
        {
            GetReferences();
        }

        public static void GetReferences()
        {
            using var context = new AppDBContext();
            _references[typeof(Unit)] = context.Units.AsNoTracking().ToList();
            _references[typeof(Customer)] = context.Customers.AsNoTracking().ToList();
            _references[typeof(Item)] = context.Items.Include(x => x.Section).Include(x => x.Unit).AsNoTracking().ToList();
            Sections = context.Sections.ToList();
        }

        public static void ReselectReferenceByName(string referenceName)
        {
            using var context = new AppDBContext();
            switch (referenceName)
            {
                case "Unit":
                    _references[typeof(Unit)] = context.Units.AsNoTracking().ToList();
                    break;
                case "Item":
                    _references[typeof(Item)] = context.Items.Include(x => x.Section).Include(x => x.Unit).AsNoTracking().ToList();
                    break;
                case "Customer":
                    _references[typeof(Customer)] = context.Customers.AsNoTracking().ToList();
                    break;
            }
        }


        public static IEnumerable<Identifier> GetReferenceByType(Type type)
        {
            _references.TryGetValue(type, out var list);
            return list ?? [];
        }

    }
}
