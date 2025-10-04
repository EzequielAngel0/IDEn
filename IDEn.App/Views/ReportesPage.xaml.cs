using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class ReportesPage : Page
    {
        public ReportesPage()
        {
            InitializeComponent();
            DataContext = new ViewModels.ReportesViewModel();
        }
    }
}
