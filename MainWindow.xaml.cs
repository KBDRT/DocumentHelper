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

    }
}