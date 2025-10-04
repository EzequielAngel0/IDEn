using System.Windows;
using System.Windows.Controls;
using IDEn.App.ViewModels;

namespace IDEn.App.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }

        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            // Key identifica el indicador para la pantalla de análisis
            var key = (string)((Button)sender).Tag;
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new AnalisisPage(key));
        }

        // Navegación del sidebar (simple por ahora)
        private void GoAnalisis(object s, RoutedEventArgs e)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new AnalisisPage());
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
