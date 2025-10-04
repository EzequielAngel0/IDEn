using System.Collections.ObjectModel;

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

    public class IdenCardVM
    {
        public IdenCardVM(string title, string unit, double value, string key)
        { Title = title; Unit = unit; Value = value; Key = key; }

        public string Title { get; }
        public string Unit { get; }
        public double Value { get; }
        public string Key { get; }   // identificador para navegar
        // opcional: string ImagePath { get; } = "Assets/iden1.jpg";
    }
}
