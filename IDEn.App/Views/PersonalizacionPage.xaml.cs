using System.Windows;
using System.Windows.Controls;
using IDEn.App.Services;

namespace IDEn.App.Views
{
    public partial class PersonalizacionPage : Page
    {
        private AppearanceSettings _vm;

        public PersonalizacionPage()
        {
            InitializeComponent();
            Loaded += async (_, __) =>
            {
                _vm = await SettingsService.Instance.GetAppearanceAsync();
                DataContext = _vm;
            };
        }

        private async void Guardar(object s, RoutedEventArgs e)
        {
            await SettingsService.Instance.SaveAppearanceAsync(_vm);
            MessageBox.Show("Personalización guardada.", "IDEn", MessageBoxButton.OK, MessageBoxImage.Information);
            // Aquí podrías aplicar el tema/color en runtime si lo deseas.
        }

        private void Back(object s, RoutedEventArgs e) => ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new ConfiguracionPage());
    }
}
