using DocumentHelper.ModelView.References;
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
