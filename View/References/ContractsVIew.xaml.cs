using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView;
using DocumentHelper.ViewModel.References.Contracts;
using System.Windows;

namespace DocumentHelper.View.References
{

    public partial class ContractsView : Window
    {
        public ContractsView(Customer? customer = null, DocumentViewModel? document = null)
        {
            InitializeComponent();
            var viewModel = new ContractsViewModel(customer, document, this);
            DataContext = viewModel;
        }
    }
}
