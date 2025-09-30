using DocumentHelper.Infrastructure.Files;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.Database;
using DocumentHelper.Services.DataServices;
using DocumentHelper.Services.Mapping;
using DocumentHelper.Services.Validation;
using DocumentHelper.View.Validation;
using DocumentHelper.ViewModel.Commands;
using FluentValidation.Results;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocumentHelper.ModelView
{
    public partial class DocumentViewModel
    {
        [Notify]
        private Document _document { get; set; } = null!;

        private readonly IRepositoryBase<Document> _repository = new RepositoryBase<Document>();
        public ObservableCollection<DocumentSectionViewModel> Sections { get; set; } = new();

        public ObservableCollection<Customer> AvailableCustomers { get; set; } = new();

        public ObservableCollection<Item> AllItems { get; set; } = new();

        public ObservableCollection<Unit> AllUnits { get; set; } = new();

        private SaveDocumentValidator validator = new SaveDocumentValidator();

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

        public Customer Customer
        {
            get => _document?.Customer;
            set
            {
                if (_document.Customer != value)
                {
                    _document.Customer = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Customer)));
                }
            }
        }

        public string Number
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
                    _document.DatePeriod.DateStart = value;
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

        public double Costs
        {
            get => _document.Costs;
            set
            {
                if (_document.Costs != value)
                {
                    _document.Costs = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Costs)));
                    RecalsCosts();
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TotalSum)));
                }
            }
        }

        public DocumentViewModel(Document? document = null)
        {
            var sections = ReferencesManager.GetReferenceByName("Section") as List<Section>;

            if (document == null)
            {
                _document = new() { Customer = new(), DatePeriod = new() { DateStart = DateTime.Now, DateEnd = DateTime.Now } };

                if (sections != null)
                {
                    foreach (var section in sections)
                    {
                        DocumentSectionViewModel newSection = new(RecalsCosts) { Section = new() { Name = section.Name, Id = section.Id } };
                        Sections.Add(newSection);
                        newSection.InitializeCommands();
                    }
                }
            }
            else
            {
                _document = document;

                if (sections != null)
                {
                    foreach (var section in sections)
                    {
                        ObservableCollection<PositionViewModel> positions = new();
                        foreach (var position in document.Positions.Where(x => x.SectionId == section.Id))
                        {
                            var positionViewModel = MappingService.Mapper.Map<PositionViewModel>(position);
                            positions.Add(positionViewModel);
                        }

                        DocumentSectionViewModel newSection = new(RecalsCosts)
                        {
                            Section = new() { Name = section.Name, Id = section.Id },
                            Positions = positions,
                        };

                        Sections.Add(newSection);
                        newSection.InitializeCommands();
                    }
                }

            }

            if (ReferencesManager.GetReferenceByName("Item") is List<Item> items)
            {
                foreach (var item in items)
                {
                    //for (int i = 0; i < 100; i++)
                    //{
                    AllItems.Add(item);
                    //}
                }
            }


            if (ReferencesManager.GetReferenceByName("Unit") is List<Unit> units)
            {
                foreach (var unit in units)
                {
                    AllUnits.Add(unit);
                }
            }

            if (ReferencesManager.GetReferenceByName("Customer") is List<Customer> customerReference)
            {
                foreach (var unit in customerReference)
                {
                    AvailableCustomers.Add(unit);
                }
            }

            SaveCommand = new(SaveCommandHandler);
            CreateFileCommand = new(CreateFileCommandHandler);
            RecalsCosts();
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

        public async void SaveCommandHandler(object parameter)
        {
            _document.Positions.Clear();
            if (_document.Customer != null)
            {
                _document.CustomerId = _document.Customer.Id;
            }
            _document.Customer = null;

            foreach (var section in Sections)
            {
                foreach(var position in section.Positions)
                {
                    var item = position.Item.GetItem();
                    var itemId = item.Id;

                    var itemFromRef = AllItems.FirstOrDefault(x => x.Name == item.Name);
                    if (itemFromRef != null)
                    {
                        itemId = itemFromRef.Id;
                        item = null;
                    }
                    else
                    {
                        item = new()
                        {
                            SectionId = section.Id,
                            UnitId = position.Unit.Id,
                            Name = item.Name,
                            Id = 0
                        };
                        itemId = 0;
                    }

                    _document.Positions.Add(new()
                    {
                        Id = position.Id,
                        Amount = position.Amount,
                        Number = position.Number,
                        Price = position.Price,
                        SectionId = section.Id,
                        ItemId = itemId,
                        Item = item,
                        DocumentId = Document.Id,
                    });
                }
            }

            if (!ValidatorsService.ValidateWithWindow(_document))
            {
                return;
            }

            await _repository.SaveDocument(_document);

            ReferencesManager.SelectReferenceByName("Document");
            ReferencesManager.SelectReferenceByName("Item");

            _document = (ReferencesManager.GetReferenceByName("Document") as List<Document>).SingleOrDefault(x => x.Id == _document.Id);
        }


        public RelayCommand CreateFileCommand { get; private set; }

        public void CreateFileCommandHandler(object parameter)
        {
            ExcelKS2Filler ExcelFiller = new ExcelKS2Filler("template.xlsx", "New.xlsx", _document);

            ExcelFiller.ProcessFile();
        }

    }
}
