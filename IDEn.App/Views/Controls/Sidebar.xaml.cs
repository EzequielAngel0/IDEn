using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IDEn.App.Views.Controls
{
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
            Loaded += (_, __) => ApplyActive();
        }

        // "Inicio", "Analisis", "Datos", "Reportes", "Configuracion"
        public string Active
        {
            get => (string)GetValue(ActiveProperty);
            set => SetValue(ActiveProperty, value);
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register(nameof(Active), typeof(string), typeof(Sidebar),
                new PropertyMetadata("Inicio", (o, e) => ((Sidebar)o).ApplyActive()));

        void ApplyActive()
        {
            BtnInicio.IsChecked = IsActive("Inicio");
            BtnAnalisis.IsChecked = IsActive("Analisis");
            BtnDatos.IsChecked = IsActive("Datos");
            BtnReportes.IsChecked = IsActive("Reportes");
            BtnConfig.IsChecked = IsActive("Configuracion");

            IconInicio.Source = LoadIcon(BtnInicio.IsChecked == true ? "home-filled.png" : "home.png");
            IconAnalisis.Source = LoadIcon(BtnAnalisis.IsChecked == true ? "analytics-filled.png" : "analytics.png");
            IconDatos.Source = LoadIcon(BtnDatos.IsChecked == true ? "database-filled.png" : "database.png");
            IconReportes.Source = LoadIcon(BtnReportes.IsChecked == true ? "report-filled.png" : "report.png");
            IconConfig.Source = LoadIcon(BtnConfig.IsChecked == true ? "settings-filled.png" : "settings.png");
        }

        bool IsActive(string v) => string.Equals(Active, v, StringComparison.OrdinalIgnoreCase);

        static BitmapImage LoadIcon(string file) =>
            new BitmapImage(new Uri($"/IDEn.App;component/Assets/Icons/{file}", UriKind.Relative));

        // Navegación
        void GoInicio(object s, RoutedEventArgs e) => Navigate(new Views.DashboardPage());
        void GoAnalisis(object s, RoutedEventArgs e) => Navigate(new Views.AnalisisPage());
        void GoDatos(object s, RoutedEventArgs e) => Navigate(new Views.DatosPage());
        void GoReportes(object s, RoutedEventArgs e) => Navigate(new Views.ReportesPage());
        void GoConfig(object s, RoutedEventArgs e) => Navigate(new Views.ConfiguracionPage());

        static void Navigate(Page page)
        {
            var win = (MainWindow)Application.Current.MainWindow;
            win.MainFrame.Navigate(page);
        }
    }
}
