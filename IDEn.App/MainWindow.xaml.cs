using System.Windows;

namespace IDEn.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Views.DashboardPage());
        }
    }
}
