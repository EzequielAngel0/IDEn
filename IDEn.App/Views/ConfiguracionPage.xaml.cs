using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class ConfiguracionPage : Page
    {
        public ConfiguracionPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.ConfiguracionViewModel();
        }
    }
}
