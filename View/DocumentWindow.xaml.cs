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
        private string _currentFilter = "";

        private PositionViewModel? _currentPosition;

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

            if (grid.CurrentCell.Column.DisplayIndex != 1)
                return;

            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                PopupSuggest.IsOpen = false;
                return;
            }

            var currentCell = grid.GetCurrentCell();
            if (currentCell?.Content is TextBox textBox)
            {
                UpdatePopupFilter(grid, textBox.Text);
            }

            _currentPosition = (PositionViewModel?)grid.CurrentCell.Item;
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
                        Unit = selectedItem.Unit,
                        Id = selectedItem.Id,
                    };

                    _currentPosition.Unit = new Unit()
                    {
                        ShortName = selectedItem?.Unit?.ShortName,
                        Id = (int)selectedItem?.Unit?.Id,
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
                ListBoxSuggest.Items.Filter = null;
            }
            else
            {
                var section = MainTabControl.SelectedValue as DocumentSectionViewModel;

                ListBoxSuggest.Items.Filter = item => item is Item dataItem 
                               && dataItem.Name.Contains(_currentFilter, StringComparison.OrdinalIgnoreCase)
                               && dataItem.Section.Id == section.Section.Id;
            }

            if (ListBoxSuggest.Items.Count > 0 && !string.IsNullOrWhiteSpace(_currentFilter))
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

    }
}
