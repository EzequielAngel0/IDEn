using System;
using System.Windows.Controls;

namespace IDEn.App.Services.Navigation
{
    public interface INavigationService
    {
        // Se llama una vez al iniciar para “conectar” el Frame de MainWindow
        void Initialize(Frame frame);

        // Navega a una Page registrada (por tipo)
        void Navigate<TPage>(object parameter = null) where TPage : Page;
        void Navigate(Type pageType, object parameter = null);

        // Navegación hacia atrás (si aplica)
        bool CanGoBack { get; }
        void GoBack();
    }
}
