using System.Windows;
using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            // Si quieres usar VM luego: DataContext = new DashboardViewModel();
        }

        private void IrAnalisis_Click(object sender, RoutedEventArgs e)
        {
            // Navegación simple usando el Frame de MainWindow
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new AnalisisPage());
        }
    }
}
