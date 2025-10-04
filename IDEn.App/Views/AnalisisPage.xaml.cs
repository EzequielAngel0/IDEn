using System.Windows.Controls;

namespace IDEn.App.Views
{
    public partial class AnalisisPage : Page
    {
        private readonly string _indicatorKey;

        public AnalisisPage(string indicatorKey = null)
        {
            InitializeComponent();
            _indicatorKey = indicatorKey;
            // TODO: usar _indicatorKey para cargar la serie correspondiente
        }
    }
}
