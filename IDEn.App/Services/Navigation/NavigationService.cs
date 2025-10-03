using System;
using System.Windows.Controls;

namespace IDEn.App.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        public void Initialize(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void Navigate<TPage>(object parameter = null) where TPage : Page
            => Navigate(typeof(TPage), parameter);

        public void Navigate(Type pageType, object parameter = null)
        {
            if (_frame == null) throw new InvalidOperationException("El Frame no ha sido inicializado.");
            var page = (Page)Activator.CreateInstance(pageType)!;
            _frame.Navigate(page, parameter);
        }

        public bool CanGoBack => _frame?.CanGoBack == true;

        public void GoBack()
        {
            if (CanGoBack) _frame.GoBack();
        }
    }
}
