using DocumentHelper.ViewModel.References.Units;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentHelper.View.References
{
    /// <summary>
    /// Логика взаимодействия для UnitView.xaml
    /// </summary>
    public partial class UnitView : Window
    {
        public UnitView()
        {
            InitializeComponent();
            DataContext = new UnitsViewModel();
        }

    

    }
}
