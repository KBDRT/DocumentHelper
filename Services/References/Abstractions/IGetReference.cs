using System.Collections;

namespace DocumentHelper.Services.References.Abstractions
{
    public interface IGetReference
    {
        public void SelectReference();

        public IEnumerable GetRecords();
    }
}
