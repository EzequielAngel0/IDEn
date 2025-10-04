using System.Windows;
using System.Windows.Controls;
using IDEn.App.Services;

namespace IDEn.App.Views
{
    public partial class UnidadesPage : Page
    {
        private UnitsSettings _vm;

        public UnidadesPage()
        {
            InitializeComponent();
            Loaded += async (_, __) =>
            {
                _vm = await SettingsService.Instance.GetUnitsAsync();
                DataContext = _vm;
            };
        }

        private async void Guardar(object s, RoutedEventArgs e)
        {
            await SettingsService.Instance.SaveUnitsAsync(_vm);
            MessageBox.Show("Unidades guardadas.", "IDEn", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Back(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
