using DocumentHelper.ViewModel.References.Customers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentHelper.View.References
{

    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }

        private void OpenDetails_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.DataContext is CustomerViewModel item)
                {
                    new AddressView(item.GetCustomer).ShowDialog();
                }
            }
            e.Handled = true;
        }

        private void OpenDetails_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.DataContext is CustomerViewModel item)
                {
                    new ContractsView(item.GetCustomer).ShowDialog();
                }
            }
            e.Handled = true;
        }
    }
}
