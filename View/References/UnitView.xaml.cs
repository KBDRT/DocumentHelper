using DocumentHelper.ModelView.References;
using System.Windows;

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
