using DocumentHelper.ModelView;
using System.Windows;

namespace DocumentHelper.View.References
{

    public partial class ItemsView : Window
    {

        public ItemsView()
        {
            InitializeComponent();
            DataContext = new ItemsViewModel();
        }

    }
}
