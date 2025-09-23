using DocumentHelper.Infrastructure.Database;
using DocumentHelper.Model;
using DocumentHelper.Model.Reference;

namespace DocumentHelper.Services
{
    public static class DataManager
    {
        public static List<Section> Sections = new();

        private static Dictionary<Type, IEnumerable<Identifier>> _references = new Dictionary<Type, IEnumerable<Identifier>>
        {
           { typeof(Unit),          new List<Unit>() },
           { typeof(Customer),      new List<Customer>() }
        };

        public static List<Unit> Units = new();


        static DataManager()
        {
            Sections.Add(new() { Name = "Монтажные работы" });
            Sections.Add(new() { Name = "Материалы" });
            Sections.Add(new() { Name = "Прочие расходы" });

            GetReferences();
        }

        public static void GetReferences()
        {
            using var context = new AppDBContext();
            _references[typeof(Unit)] = context.Units.ToList();
            Units = context.Units.ToList();
            _references[typeof(Customer)] = context.Customers.ToList();
        }

        public static IEnumerable<Identifier> GetReferenceByType(Type type)
        {
            _references.TryGetValue(type, out var list);
            return list ?? [];
        }

    }
}
