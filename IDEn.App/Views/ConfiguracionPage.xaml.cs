using System.Windows;
using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class ConfiguracionPage : Page
    {
        public ConfiguracionPage() { InitializeComponent(); }

        private void GoInicio(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new DashboardPage());
        private void GoDatos(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new DatosPage());
        private void GoAnalisis(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new AnalisisPage());
        private void GoReportes(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ReportesPage());

        private void OpenUsuarios(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new UsuariosPage());
        private void OpenUnidades(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new UnidadesPage());
        private void OpenUmbrales(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new UmbralesPage());
        private void OpenPersonalizacion(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new PersonalizacionPage());
    }
}
