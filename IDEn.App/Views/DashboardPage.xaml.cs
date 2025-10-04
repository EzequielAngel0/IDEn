using System.Windows;
using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.DashboardViewModel();
        }

        private void VerDetalles_Click(object sender, RoutedEventArgs e)
        {
            var key = (string)((Button)sender).Tag;
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(new AnalisisPage(key));
        }
    }
}
