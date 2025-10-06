
using ClosedXML.Excel;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Services.DataServices;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Windows;

namespace DocumentHelper.Infrastructure.Files
{
    public class ExcelKS2Filler : FileFiller<Document>, IDisposable
    {
        private XLWorkbook _Wbook;
        private IXLWorksheet _FirstSheet;
        private DateOnly _DateTimeSheet;
        public ExcelKS2Filler(string TemplateName, string NewFileName, Document info) : base(TemplateName, NewFileName, info)
        {
        }

        public void Dispose()
        {
            _Wbook.Dispose();
        }

        protected override void FillFile()
        {

            _FirstSheet.Cell(7, "C").Value = _Info.Customer?.Name;
            _FirstSheet.Cell(11, "C").Value = _Info.Building;

            _FirstSheet.Cell(21, "M").Value = _Info.Number;
            _FirstSheet.Cell(21, "N").Value = _Info.Date.ToString("dd.MM.yyyy");
            _FirstSheet.Cell(21, "O").Value = _Info.DatePeriod.DateStart.ToString("dd.MM.yyyy");
            _FirstSheet.Cell(21, "P").Value = _Info.DatePeriod.DateEnd.ToString("dd.MM.yyyy");
            _FirstSheet.Cell(13, "C").Value = _Info.Address?.FullAddress;

            _FirstSheet.Cell(15, "N").Value = _Info.Contract?.Number;
            _FirstSheet.Cell(16, "N").Value = _Info.Contract?.Date.ToString("dd.MM.yyyy");
            //_FirstSheet.Cell(17, "N").Value = _Info.Contract?.Type;

            int startRowForSection = 31;
            var RangeToCopy = _FirstSheet.Range(startRowForSection, 1, startRowForSection, _FirstSheet.ColumnCount());


            int rowCounter = startRowForSection;

            var sections = ReferencesManager.GetReferenceByName("Section") as List<Section>;
            //sections = (List<Section>?)sections.OrderBy(x => x.Number);

            int positionCounter = 0;

            double firstSectionSum = 0;
            double otherSectionsSum = 0;
            foreach (var section in sections)
            {
                var positionsForSection = _Info.Positions.Where(x => x.SectionId == section.Id);
                section.TotalSum = 0;
                if (positionsForSection != null)
                {
                    int positionsSectionCount = positionsForSection.Count();
                    if (positionsSectionCount > 0)
                    { 
                        _FirstSheet.Row(rowCounter).InsertRowsBelow(positionsSectionCount);
                        _FirstSheet.Rows(rowCounter, rowCounter + positionsSectionCount - 1).Height = 22.5;
                    }

                    foreach (var position in positionsForSection)
                    {
                        rowCounter++;
                        positionCounter++;
                        var newRange = _FirstSheet.Cell(rowCounter, 1);
                        RangeToCopy.CopyTo(newRange);

                        _FirstSheet.Cell(rowCounter, "A").Value = positionCounter;
                        _FirstSheet.Cell(rowCounter, "C").Value = position?.Item?.Name;
                        _FirstSheet.Cell(rowCounter, "F").Value = position?.Unit?.ShortName;
                        _FirstSheet.Cell(rowCounter, "H").Value = position?.Amount;
                        _FirstSheet.Cell(rowCounter, "I").Value = position?.Price;

                        // vilka
                        if (section.Number == 1)
                        {
                            _FirstSheet.Cell(rowCounter, "K").Value = position?.TotalSum;
                            firstSectionSum += position.TotalSum;
                        }
                        else
                        {
                            _FirstSheet.Cell(rowCounter, "J").Value = position?.TotalSum;
                            otherSectionsSum += position.TotalSum;
                        }
                        section.TotalSum += position.TotalSum;
                    }
                }
                rowCounter++;
            }

            RangeToCopy.Delete(XLShiftDeletedCells.ShiftCellsUp);

            _FirstSheet.Cell(rowCounter, "K").Value = firstSectionSum;
            _FirstSheet.Cell(rowCounter, "J").Value = otherSectionsSum;

            rowCounter++;
            foreach (var section in sections)
            {
                _FirstSheet.Cell(rowCounter + section.TemplateTotalRow, "J").Value = section.TotalSum;
            }

            rowCounter += sections.Count();
            _FirstSheet.Cell(rowCounter, "J").Value = _Info.Costs;

            rowCounter++;
            _FirstSheet.Cell(rowCounter, "J").Value = _Info.TotalSum;
        }

        protected override void OpenFile()
        {
            _Wbook = new XLWorkbook(_TemplateName);
            _FirstSheet = _Wbook?.Worksheet(1);
        }

        protected override void SaveFile()
        {
            try
            {
                string directory = Path.GetDirectoryName(_NewFileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_NewFileName);
                string extension = Path.GetExtension(_NewFileName);

                string newFilePath = _NewFileName;

                if (File.Exists(newFilePath))
                {
                    string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    newFilePath = Path.Combine(directory, $"{fileNameWithoutExtension} ({timestamp}){extension}");
                }

                _Wbook.SaveAs(newFilePath);

                MessageBoxResult result = MessageBox.Show(
                   $"Файл успешно сохранен:\n{newFilePath}\n\nХотите открыть файл?",
                   "Сохранение завершено",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    newFilePath = newFilePath.Replace("/", "\\");
                    OpenFile(newFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить файл файл:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}
