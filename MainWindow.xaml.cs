using DocumentHelper.Model.Document;
using DocumentHelper.Services.DataServices;
using DocumentHelper.View.References;
using DocumentHelper.ViewModel.References.Customers;
using DocumentHelper.ViewModel.References.Documents;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DocumentHelper
{
    
    public partial class MainWindow : Window
    {

        //public ObservableCollection<Document> Documents { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DocumentsViewModel(); 
            //LoadDocuments();

            //ReferencesManager.ReferenceUpdated += OnReferenceUpdated;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DocumentWindow().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new CustomersView().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new UnitView().Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new ItemsView().Show();
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid?.SelectedValue is DocumentMainViewModel doc)
            {
                new DocumentWindow(doc.Id).Show();
            }
        }

        private void OnReferenceUpdated(object sender, string referenceName)
        {
            //if (referenceName == "Document")
            //{
            //    LoadDocuments();
            //}
        }

        //private void LoadDocuments()
        //{
        //    if (this.DataContext is DocumentsViewModel viewModel)
        //    {
        //        Dispatcher.Invoke(() =>
        //        {
        //            if (ReferencesManager.GetReferenceByName("Document") is List<Document> documentsList)
        //            {
        //                viewModel.Documents.Clear();
        //                foreach (var doc in documentsList)
        //                {
        //                    viewModel.Documents.Add(doc);
        //                }
        //            }
        //        });
        //    }
        //}
    }
}