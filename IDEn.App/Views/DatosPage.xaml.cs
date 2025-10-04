using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class DatosPage : Page
    {
        public DatosPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.DatosViewModel();
        }
    }
}
