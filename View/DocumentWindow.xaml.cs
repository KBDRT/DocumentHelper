using DocumentHelper.Extensions;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.ModelView;
using DocumentHelper.Services;
using DocumentHelper.Services.DataServices;
using DocumentHelper.ViewModel.References.Units;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentHelper
{
    public partial class DocumentWindow : Window
    {
        private int _prevPopupRowIndex = -1;
        private int _prevPopupRowIndexUnit = -1;
        private string _currentFilter = "";

        private PositionViewModel? _currentPosition;

        private PositionViewModel? _currentPositionUnit;

        public DocumentWindow(int documentId = 0)
        {
            InitializeComponent();

            Model.Document.Document? document = null;
            if (documentId != 0)
            {
                document = (ReferencesManager.GetReferenceByName("Document") as List<Model.Document.Document>).SingleOrDefault(x => x.Id == documentId);   
            }

            DataContext = new DocumentViewModel(document);

            if (DataContext is DocumentViewModel viewModel)
            {
                ListBoxSuggest.ItemsSource = viewModel.AllItems;
                ListBoxSuggestUnit.ItemsSource = viewModel.AllUnits;
            }
        }

        private void TextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ModelView.DocumentViewModel viewModel)
            {
                viewModel.Costs = viewModel.CostsReal;
                viewModel.RecalsCosts();
            }
        }

        private void DataGridPositions_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (sender is not DataGrid grid || grid.CurrentItem == null)
                return;

            if (grid.CurrentCell.Column.DisplayIndex != 1 && grid.CurrentCell.Column.DisplayIndex != 2)
                return;

            var columnIndex = grid.CurrentCell.Column.DisplayIndex;

            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                if (columnIndex == 1)
                {
                    PopupSuggest.IsOpen = false;
                }
                else
                {
                    PopupSuggestUnit.IsOpen = false;
                }

                return;
            }

           
            if (columnIndex == 1)
            {
                ListBoxSuggest.SelectedItem = null;
            }
            else
            {
                ListBoxSuggestUnit.SelectedItem = null;
            }

            var currentCell = grid.GetCurrentCell();
            if (currentCell?.Content is TextBox textBox)
            {

                if (columnIndex == 1)
                {
                    UpdatePopupFilter(grid, textBox.Text);
                    _currentPosition = (PositionViewModel?)grid.CurrentCell.Item;
                }
                else
                {
                    UpdatePopupFilterUnit(grid, textBox.Text);
                    _currentPositionUnit = (PositionViewModel?)grid.CurrentCell.Item;
                }  
            }
        }

        private void DataGridPositions_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is DataGrid grid && grid.CurrentCell.Column.DisplayIndex == 0)
            {
                UpdatePopupFilter(grid, e.Text);
            }
        }

        private void ListBoxSuggest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxSuggest.SelectedItem is Item selectedItem)
            {
                if (_currentPosition != null)
                {
                    _currentPosition.Position.Item = selectedItem;
                    _currentPosition.Item = new ItemViewModel()
                    { 
                        Name = selectedItem.Name,
                        Id = selectedItem.Id,
                    };
                    _currentPosition = null;
                }
                PopupSuggest.IsOpen = false;

                //DataGridPositions.Focus();
            }
        }

        private void UpdatePopupFilter(DataGrid grid, string filterText)
        {
            if (grid.CurrentItem == null) return;

            int currentRowIndex = grid.Items.IndexOf(grid.CurrentItem);
            var currentCell = grid.GetCurrentCell();

            if (currentCell == null) return;

            _currentFilter = filterText ?? "";

            if (string.IsNullOrWhiteSpace(_currentFilter))
            {
                var section = MainTabControl.SelectedValue as DocumentSectionViewModel;

                ListBoxSuggest.Items.Filter = item => item is Item dataItem
                              && dataItem.Section.Id == section.Section.Id;
            }
            else
            {
                var section = MainTabControl.SelectedValue as DocumentSectionViewModel;

                ListBoxSuggest.Items.Filter = item => item is Item dataItem 
                               && dataItem.Name.Contains(_currentFilter, StringComparison.OrdinalIgnoreCase)
                               && dataItem.Section.Id == section.Section.Id;
            }

            if (ListBoxSuggest.Items.Count > 0)
            {
                if (_prevPopupRowIndex != currentRowIndex || !PopupSuggest.IsOpen)
                {
                    PopupSuggest.PlacementTarget = currentCell;
                    PopupSuggest.Width = currentCell.ActualWidth;
                    PopupSuggest.IsOpen = true;
                    _prevPopupRowIndex = currentRowIndex;
                }
            }
            else
            {
                PopupSuggest.IsOpen = false;
            }

            //ListBoxSuggest.Items.Refresh();
        }

        private void UpdatePopupFilterUnit(DataGrid grid, string filterText)
        {
            if (grid.CurrentItem == null) return;

            int currentRowIndex = grid.Items.IndexOf(grid.CurrentItem);
            var currentCell = grid.GetCurrentCell();

            if (currentCell == null) return;

            _currentFilter = filterText ?? "";

            if (string.IsNullOrWhiteSpace(_currentFilter))
            {
                ListBoxSuggestUnit.Items.Filter = null;
            }
            else
            {
                var section = MainTabControl.SelectedValue as DocumentSectionViewModel;

                ListBoxSuggestUnit.Items.Filter = unit => unit is Unit dataUnit
                               && dataUnit.ShortName.Contains(_currentFilter, StringComparison.OrdinalIgnoreCase);
            }

            if (ListBoxSuggestUnit.Items.Count > 0)
            {
                if (_prevPopupRowIndexUnit != currentRowIndex || !PopupSuggest.IsOpen)
                {
                    PopupSuggestUnit.PlacementTarget = currentCell;
                    PopupSuggestUnit.Width = currentCell.ActualWidth;
                    PopupSuggestUnit.IsOpen = true;
                    _prevPopupRowIndexUnit = currentRowIndex;
                }
            }
            else
            {
                PopupSuggestUnit.IsOpen = false;
            }

            //ListBoxSuggest.Items.Refresh();
        }

        private void ListBoxSuggestUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxSuggestUnit.SelectedItem is Unit selectedUnit)
            {
                if (_currentPositionUnit != null)
                {
                    _currentPositionUnit.Position.Unit = selectedUnit;
                    _currentPositionUnit.Unit = new Unit()
                    {
                        ShortName = selectedUnit.ShortName,
                        Id = selectedUnit.Id,
                    };
                    _currentPositionUnit = null;
                }
                PopupSuggestUnit.IsOpen = false;
            }
        }
    }
}
