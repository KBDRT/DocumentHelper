using DocumentHelper.Model.Document;
using DocumentHelper.Services.DataServices;
using DocumentHelper.View.References;
using DocumentHelper.ViewModel.References.Customers;
using DocumentHelper.ViewModel.References.Documents;
using System.Windows;
using System.Windows.Controls;

namespace DocumentHelper
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DocumentsViewModel(); 
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
    }
}