using DocumentHelper.View.References;
using System.Windows;

namespace DocumentHelper
{
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
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
    }
}