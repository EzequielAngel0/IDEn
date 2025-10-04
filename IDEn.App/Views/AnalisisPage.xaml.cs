using System.Windows;
using System.Windows.Controls;
using IDEn.App.ViewModels;

namespace IDEn.App.Views
{
    public partial class AnalisisPage : Page
    {
        public AnalisisPage(string indicatorKey = null)
        {
            InitializeComponent();
            DataContext = new AnalisisViewModel(indicatorKey);
        }

        // Navegación simple del sidebar
        private void GoInicio(object s, RoutedEventArgs e)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new DashboardPage());
        }
        private void GoDatos(object s, RoutedEventArgs e)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new DatosPage());
        }
        private void GoReportes(object s, RoutedEventArgs e)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new ReportesPage());
        }
        private void GoConfig(object s, RoutedEventArgs e)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new ConfiguracionPage());
        }
    }
}
