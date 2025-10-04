using System.Windows;
using IDEn.App.Views;

namespace IDEn.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());
        }
    }
}
