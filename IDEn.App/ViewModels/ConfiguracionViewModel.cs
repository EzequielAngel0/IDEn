using System.ComponentModel;

namespace IDEn.App.ViewModels
{
    public sealed class ConfiguracionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Ejemplo de propiedades configurables
        private string _tema = "Claro";
        public string Tema
        {
            get => _tema;
            set { if (_tema != value) { _tema = value; OnPropertyChanged(nameof(Tema)); } }
        }

        private string _unidadEnergia = "kWh";
        public string UnidadEnergia
        {
            get => _unidadEnergia;
            set { if (_unidadEnergia != value) { _unidadEnergia = value; OnPropertyChanged(nameof(UnidadEnergia)); } }
        }
    }
}
