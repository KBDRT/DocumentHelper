using DocumentHelper.ViewModel.References.Customers;
using System.Windows;

namespace DocumentHelper.View.References
{

    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }
    }
}
