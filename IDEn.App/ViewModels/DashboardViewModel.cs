using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IDEn.App.ViewModels
{
    public class DashboardViewModel
    {
        public ObservableCollection<IdenCardVM> Cards { get; } = new()
        {
            new IdenCardVM("IDEn Electricidad Total",          "kWh/pza", 0.1711, "electricidad_total"),
            new IdenCardVM("IDEn Electricidad Transformador 2","kWh/pza", 0.0783, "electricidad_t2"),
            new IdenCardVM("IDEn Gas Natural",                 "kWh/ciclo", 4956, "gas_natural")
        };

        public int SelectedYear { get; set; } = 2024;
        public int[] AvailableYears { get; } = new[] { 2023, 2024, 2025 };
    }

    // ← Reemplaza esta clase por completo
    public class IdenCardVM : INotifyPropertyChanged
    {
        private string _title;
        private string _unit;
        private double _value;
        private string _key;

        public IdenCardVM(string title, string unit, double value, string key)
        {
            _title = title; _unit = unit; _value = value; _key = key;
        }

        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Unit { get => _unit; set { _unit = value; OnPropertyChanged(); } }
        public double Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public string Key { get => _key; set { _key = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
