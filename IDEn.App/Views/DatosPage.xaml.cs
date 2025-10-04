using System.Windows;
using System.Windows.Controls;
using IDEn.App.ViewModels;

namespace IDEn.App.Views
{
    public partial class DatosPage : Page
    {
        public DatosPage()
        {
            InitializeComponent();
            DataContext = new DatosViewModel();
        }

        // Navegación simple con el Frame del MainWindow
        private void GoInicio(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new DashboardPage());

        private void GoAnalisis(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new AnalisisPage());

        private void GoReportes(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ReportesPage());

        private void GoConfig(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
