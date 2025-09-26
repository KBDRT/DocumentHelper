using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentHelper
{
    /// <summary>
    /// Логика взаимодействия для TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        private Window? _window;

        public static readonly DependencyProperty MyPropertyProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(TitleBar), new UIPropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }


        public TitleBar()
        {
            InitializeComponent();
            this.Loaded += OnTitleBarLoaded;
            DataContext = this;
        }

        private void OnTitleBarLoaded(object sender, RoutedEventArgs e)
        {
            _window = Window.GetWindow(this);
        }

        private void OnWindowInitialized(object sender, EventArgs e)
        {
            RefreshMaximizeRestoreButton();
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (_window.WindowState == WindowState.Maximized)
            {
                _window.WindowState = WindowState.Normal;
            }
            else if (_window.WindowState == WindowState.Normal)
            {
                _window.WindowStyle = WindowStyle.SingleBorderWindow;
                _window.WindowState = WindowState.Maximized;
                _window.WindowStyle = WindowStyle.None;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            _window.Close();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (_window.WindowState == WindowState.Maximized)
            {
                MaximizeButton.Visibility = Visibility.Collapsed;
                RestoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                MaximizeButton.Visibility = Visibility.Visible;
                RestoreButton.Visibility = Visibility.Collapsed;


            }
        }

        private void WindowStateChangedHandler(object sender, EventArgs e)
        {
            RefreshMaximizeRestoreButton();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _window.DragMove();
        }
    }
}
