using System.Windows;
using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class UsuariosPage : Page
    {
        public UsuariosPage() { InitializeComponent(); }
        private void Back(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
