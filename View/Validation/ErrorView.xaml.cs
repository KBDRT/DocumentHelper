using FluentValidation.Results;
using System.Windows;

namespace DocumentHelper.View.Validation
{

    public partial class ErrorWindow : Window
    {
        public ErrorWindow(List<ValidationFailure> errors)
        {
            InitializeComponent();
            ErrorsGrid.ItemsSource = errors;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
