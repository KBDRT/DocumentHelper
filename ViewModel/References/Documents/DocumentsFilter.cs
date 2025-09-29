using PropertyChanged.SourceGenerator;
using System.ComponentModel;

namespace DocumentHelper.ViewModel.References.Documents
{
    public partial class DocumentsFilter
    {
        [Notify]
        private string _number = string.Empty;

        [Notify]
        private DateTime _dateStart = DateTime.MinValue;

        [Notify]
        private DateTime _dateEnd = DateTime.MaxValue;

        public bool Apply(object obj)
        {
            bool isSuccessFilter = true;
            if (obj is not DocumentMainViewModel document)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(Number))
            {
                isSuccessFilter = document.Number.StartsWith(Number);
            }

            if (isSuccessFilter)
            {
                isSuccessFilter = document.Date >= DateStart && document.Date <= DateEnd;
            }


            //if (obj is Person person)
            //{
            //    return person.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
            //           person.Age.ToString().Contains(FilterText);
            //}

            return isSuccessFilter;
        }

        public void Clear()
        {
            Number = string.Empty;
            DateStart = DateTime.MinValue;
            DateEnd = DateTime.MaxValue;
        }
    }
}
