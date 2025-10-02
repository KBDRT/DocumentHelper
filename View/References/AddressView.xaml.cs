using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView;
using DocumentHelper.ViewModel.References.Addresses;
using DocumentHelper.ViewModel.References.Units;
using System.Windows;

namespace DocumentHelper.View.References
{
    /// <summary>
    /// Логика взаимодействия для UnitView.xaml
    /// </summary>
    public partial class AddressView : Window
    {
        public AddressView(Customer? customer = null, DocumentViewModel? document = null)
        {
            InitializeComponent();
            var viewModel = new AddressesViewModel(customer, document, this);
            DataContext = viewModel;
        }
    }
}
