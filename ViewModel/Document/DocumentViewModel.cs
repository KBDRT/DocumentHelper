using DocumentHelper.Infrastructure.Files;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.Services.Database;
using DocumentHelper.Services.DataServices;
using DocumentHelper.Services.Mapping;
using DocumentHelper.Services.Validation;
using DocumentHelper.View.References;
using DocumentHelper.View.Validation;
using DocumentHelper.ViewModel.Commands;
using FluentValidation.Results;
using PropertyChanged.SourceGenerator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocumentHelper.ModelView
{
    public partial class DocumentViewModel
    {
        [Notify]
        private Document _document { get; set; } = null!;

        private readonly IRepositoryBase<Document> _repository = new RepositoryBase<Document>();

        private readonly IRepositoryBase<Unit> _repositoryUnit = new RepositoryBase<Unit>();

        public ObservableCollection<DocumentSectionViewModel> Sections { get; set; } = new();

        public ObservableCollection<Customer> AvailableCustomers => new(ReferencesManager.GetReferenceByName("Customer") as List<Customer>);

        public ObservableCollection<Item> AllItems => new(ReferencesManager.GetReferenceByName("Item") as List<Item>);

        public ObservableCollection<Unit> AllUnits => new(ReferencesManager.GetReferenceByName("Unit") as List<Unit>);

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

        public Contract Contract
        {
            get => _document?.Contract;
            set
            {
                if (_document.Contract != value)
                {
                    _document.Contract = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Contract)));
                }
            }
        }

        public Address Address
        {
            get => _document?.Address;
            set
            {
                if (_document.Address != value)
                {
                    _document.Address = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Address)));
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

        public string Building
        {
            get => _document.Building;
            set
            {
                if (_document.Building != value)
                {
                    _document.Building = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Building)));
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

            //FillRef();

            SaveCommand = new(SaveCommandHandler);
            CreateFileCommand = new(CreateFileCommandHandler);
            OpenWindowCommand = new(OpenWindowCommandHandler);
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

        //private void FillRef()
        //{
        //    AllItems.Clear();
        //    if (ReferencesManager.GetReferenceByName("Item") is List<Item> items)
        //    {
        //        foreach (var item in items)
        //        {
        //            AllItems.Add(item);
        //        }
        //    }

        //    AllUnits.Clear();
        //    if (ReferencesManager.GetReferenceByName("Unit") is List<Unit> units)
        //    {
        //        foreach (var unit in units)
        //        {
        //            AllUnits.Add(unit);
        //        }
        //    }

        //    AvailableCustomers.Clear();
        //    if (ReferencesManager.GetReferenceByName("Customer") is List<Customer> customerReference)
        //    {
        //        foreach (var unit in customerReference)
        //        {
        //            AvailableCustomers.Add(unit);
        //        }
        //    }
        //}




        public RelayCommand SaveCommand { get; private set; }

        public async void SaveCommandHandler(object parameter)
        {
            _document.Positions.Clear();

            if (_document.Customer != null)
            {
                _document.CustomerId = _document.Customer.Id;
            }

            if (_document.Address != null)
            {
                _document.AddressId = _document.Address.Id;
            }
            
            if (_document.Contract != null)
            {
                _document.ContractId = _document.Contract.Id;
            }

            var allUnits = Sections.SelectMany(s => s.Positions).Select(p => p.Unit).GroupBy(x => x.ShortName).Select(g => g.First()).ToList();


            foreach (var unit in allUnits)
            {
                var unitFromRef = AllUnits.FirstOrDefault(x => x.ShortName == unit.ShortName);
                if (unitFromRef == null)
                {
                    unit.Id = 0;
                }
            }

            await _repositoryUnit.SaveUnits(allUnits);
            ReferencesManager.SelectReferenceByName("Unit");

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
                            //UnitId = position.Unit.Id,
                            Name = item.Name,
                            Id = 0
                        };
                        itemId = 0;
                    }

                    var unit = position.Unit;
                    var unitId = unit.Id;
                    var unitFromRef = AllUnits.FirstOrDefault(x => x.ShortName == unit.ShortName);
                    if (unitFromRef != null)
                    {
                        unitId = unitFromRef.Id;
                        unit = null;
                    }
                    else
                    {
                        unit = new()
                        {
                            ShortName = unit.ShortName,
                            Id = 0
                        };
                        unitId = 0;
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
                        Unit = unit,
                        UnitId = unitId
                    });
                }
            }

            if (!ValidatorsService.ValidateWithWindow(_document))
            {
                return;
            }
            _document.Customer = null;
            _document.Address = null;
            _document.Contract = null;


            await _repository.SaveDocument(_document);

            ReferencesManager.SelectReferenceByName("Document");
            ReferencesManager.SelectReferenceByName("Item");
            //ReferencesManager.SelectReferenceByName("Unit");

            _document = (ReferencesManager.GetReferenceByName("Document") as List<Document>).SingleOrDefault(x => x.Id == _document.Id);
        }


        public RelayCommand CreateFileCommand { get; private set; }

        public void CreateFileCommandHandler(object parameter)
        {
            string documentDate = _document.Date.ToString("dd-MM-yyyy");
            var newFileName = $"КС-2/{_document?.Customer?.Name} №{_document?.Number} {documentDate}.xlsx";

            ExcelKS2Filler ExcelFiller = new ExcelKS2Filler("template.xlsx", newFileName, _document);

            ExcelFiller.ProcessFile();
        }

        public RelayCommand OpenWindowCommand { get; private set; }

        public void OpenWindowCommandHandler(object parameter)
        {
            if (parameter == null) { return; }

            if (_document.Customer?.Id == 0)
            {
                MessageBox.Show("Необходимо выбрать заказчика!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (parameter.ToString() == "Address")
            {
                new AddressView(_document.Customer, this).ShowDialog();
            }
            else if (parameter.ToString() == "Contract")
            {
                new ContractsView(_document.Customer, this).ShowDialog();
            }
        }


    }
}
