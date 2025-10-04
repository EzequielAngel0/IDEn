using System.Windows;
using System.Windows.Controls;
using IDEn.App.Services;

namespace IDEn.App.Views
{
    public partial class UmbralesPage : Page
    {
        private AlertThresholds _vm;

        public UmbralesPage()
        {
            InitializeComponent();
            Loaded += async (_, __) =>
            {
                _vm = await SettingsService.Instance.GetAlertsAsync();
                DataContext = _vm;
            };
        }

        private async void Guardar(object s, RoutedEventArgs e)
        {
            await SettingsService.Instance.SaveAlertsAsync(_vm);
            MessageBox.Show("Umbrales guardados.", "IDEn", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Back(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
