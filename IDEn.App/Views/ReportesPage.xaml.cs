using System.Windows;
using System.Windows.Controls;
using IDEn.App.ViewModels;

namespace IDEn.App.Views
{
    public partial class ReportesPage : Page
    {
        public ReportesPage()
        {
            InitializeComponent();
            DataContext = new ReportesViewModel();
        }

        private void GoInicio(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new DashboardPage());
        private void GoDatos(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new DatosPage());
        private void GoAnalisis(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new AnalisisPage());
        private void GoConfig(object s, RoutedEventArgs e)
            => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
