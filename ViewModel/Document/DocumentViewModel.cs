using DocumentHelper.Commands;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.Services;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DocumentHelper.ModelView
{
    public partial class DocumentViewModel
    {
        [Notify]
        private Document _document { get; set; } = null!;

        private readonly ReferenceService<Document> _service = new();
        public ObservableCollection<DocumentSectionViewModel> Sections { get; set; } = new();

        public ObservableCollection<Customer> AvailableCustomers { get; set; } = new();

        public ObservableCollection<Item> AllItems { get; set; } = new();

        public ObservableCollection<Unit> AllUnits { get; set; } = new();

        [Notify]
        private double _costs;

        [Notify]
        private double _costsReal;

        [Notify]
        private double _totalSum = 0;

        public string CostsTooltip
        {
            get
            {
                var totalSum = Sections.Sum(x => x.SectionTotalSum);
                var procent = 5;

                return $"Сумма по разделам: {totalSum:N2} ₽\n" +
                       $"Ставка: {procent}%\n" +
                       $"Итого: {CostsReal:N2} ₽";
            }
        }

        public int Number
        {
            get => _document.Number;
            set
            {
                if (_document.Number != value)
                {
                    _document.Number = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }

        public DateTime Date
        {
            get => _document.Date;
            set
            {
                if (_document.Date != value)
                {
                    _document.Date = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        public DateTime DateStart
        {
            get => _document.DatePeriod.DateStart;
            set
            {
                if (_document.DatePeriod.DateStart != value)
                {
                    _document.Date = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(DateStart)));
                }
            }
        }

        public DateTime DateEnd
        {
            get => _document.DatePeriod.DateEnd;
            set
            {
                if (_document.DatePeriod.DateEnd != value)
                {
                    _document.DatePeriod.DateEnd = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(DateEnd)));
                }
            }
        }

        public DocumentViewModel()
        {
            _document = new() { DatePeriod = new() { DateStart = DateTime.Now, DateEnd = DateTime.Now} };

            foreach (var item in DataManager.GetReferenceByType(typeof(Item)) as List<Item>)
            {
                for (int i = 0; i < 100; i++)
                {
                    AllItems.Add(item);
                }
            }

            foreach (var unit in DataManager.GetReferenceByType(typeof(Unit)) as List<Unit>)
            {
                AllUnits.Add(unit);
            }

            foreach (var section in DataManager.Sections)
            {
                DocumentSectionViewModel newSection = new(RecalsCosts) { Section = new() { Name = section.Name, Id = section.Id } };
                Sections.Add(newSection);
                newSection.InitializeCommands();
            }

            if (DataManager.GetReferenceByType(typeof(Customer)) is List<Customer> customerReference)
            {
                foreach (var unit in customerReference)
                {
                    AvailableCustomers.Add(unit);
                }
            }

            SaveCommand = new(SaveCommandHandler);
        }

        public void OnCostsChanged()
        {
            TotalSum = Sections.Sum(x => x.SectionTotalSum) + Costs;
        }

        public void RecalsCosts()
        {
            CostsReal = Sections.Sum(x => x.SectionTotalSum) * 0.05;
            TotalSum = Sections.Sum(x => x.SectionTotalSum) + Costs;
        }

        public RelayCommand SaveCommand { get; private set; }

        public void SaveCommandHandler(object parameter)
        {
            foreach (var section in Sections)
            {
                foreach(var position in section.Positions)
                {
                    var item = position.Item.GetItem();
                    item.SectionId = section.Id;
                    item.UnitId = item.Unit.Id;
                    item.Unit = null;

                    _document.Positions.Add(new()
                    {
                        Id = position.Id,
                        Amount = position.Amount,
                        Number = position.Number,
                        Price = position.Price,
                        SectionId = section.Id,
                        Item = item,
                        DocumentId = Document.Id,
                    });
                }
            }

            _service.SaveDocument(_document);
        }


    }
}
