using DocumentHelper.Model.Reference;
using DocumentHelper.ModelView;
using DocumentHelper.ViewModel.References.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (sender is DataGridRow row)
            //{
            //    if (DataContext is ContractsViewModel viewModel && row.DataContext is ContractViewModel viewModelContract)
            //    {
            //        if (viewModel.IsVisibleSelectButton == Visibility.Visible)
            //        {
            //            viewModel.SelectedItem = viewModelContract;
            //            viewModel.SelectCommand.Execute(new());
            //        }
            //    }
            //}
        }

    }
}
