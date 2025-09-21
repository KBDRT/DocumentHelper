using DocumentHelper.ModelView;
using System.Windows;
using System.Windows.Input;

namespace DocumentHelper
{
    public partial class DocumentWindow : Window
    {

        public DocumentWindow()
        {
            InitializeComponent();
            DataContext = new DocumentViewModel();
        }

        private void TextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DocumentViewModel viewModel)
            {
                viewModel.Costs = viewModel.CostsReal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
