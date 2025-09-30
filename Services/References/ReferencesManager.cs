
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.References.Abstractions;
using DocumentHelper.Services.References.SpecificReferences;
using System.Collections;

namespace DocumentHelper.Services.DataServices
{
    public static class ReferencesManager
    {
        private static Dictionary<string, IGetReference> _references = new()
        {
            { "Unit", new Reference<Unit>() },
            { "Customer", new Reference<Customer>() },
            { "Item", new ItemsReference() },
            { "Document", new DocumentsReference() },
            { "Section", new Reference<Section>() },
        };

        static ReferencesManager()
        {
            SelectAllReferences();
        }

        public static void SelectAllReferences()
        {
            foreach (var reference in _references)
            {
                reference.Value.SelectReference();
            }
            UpdateReferences();
        }

        public static IEnumerable? GetReferenceByName(string referenceName)
        {
            var isReferenceValid = _references.TryGetValue(referenceName, out var reference);
            if (isReferenceValid && reference != null)
            {
                return reference.GetRecords();
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable? GetReferenceByType<T>()
        {
            var typeName = typeof(T).Name;
            return GetReferenceByName(typeName);
        }

        public static void SelectReferenceByName(string referenceName)
        {
            var isReferenceValid = _references.TryGetValue(referenceName, out var reference);
            if (isReferenceValid && reference != null)
            {
                reference.SelectReference();
                UpdateReference(referenceName);
            }
        }

        public static event EventHandler? ReferencesUpdated;

        public static void UpdateReferences()
        {
            ReferencesUpdated?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler<string> ReferenceUpdated;

        public static void UpdateReference(string referenceName)
        {
            ReferenceUpdated?.Invoke(null, referenceName);
        }
    }
}
