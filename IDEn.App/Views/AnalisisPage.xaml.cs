using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class AnalisisPage : Page
    {
        public AnalisisPage() : this(null) { }

        public AnalisisPage(string? indicatorKey)
        {
            InitializeComponent();
            DataContext = new ViewModels.AnalisisViewModel(indicatorKey);
        }
    }
}
