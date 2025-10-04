using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IDEn.App.Services
{
    public class AppSettings
    {
        public UnitsSettings Units { get; set; } = new();
        public AlertThresholds Alerts { get; set; } = new();
        public AppearanceSettings Appearance { get; set; } = new();
    }

    public class UnitsSettings
    {
        public string UnidadEnergia { get; set; } = "kWh";
        public string UnidadGas { get; set; } = "GJ";
        public string UnidadProduccion { get; set; } = "unidades";
    }

    public class AlertThresholds
    {
        public double IdenMax { get; set; } = 1.50;
        public double KwhTotalMax { get; set; } = 100_000;
        public double GasGJMax { get; set; } = 5_000;
    }

    public class AppearanceSettings
    {
        public string Tema { get; set; } = "Claro";         // Claro / Oscuro
        public string ColorPrimario { get; set; } = "#2563EB";
    }

    /// <summary>Servicio simple para cargar/guardar settings.json en %AppData%\IDEn\</summary>
    public class SettingsService
    {
        private static readonly Lazy<SettingsService> _lazy = new(() => new SettingsService());
        public static SettingsService Instance => _lazy.Value;

        private readonly string _folder;
        private readonly string _file;

        private AppSettings _settings = new();

        private SettingsService()
        {
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IDEn");
            _file = Path.Combine(_folder, "settings.json");
        }

        public async Task<AppSettings> LoadAsync()
        {
            try
            {
                if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
                if (!File.Exists(_file))
                {
                    await SaveAsync(_settings);
                    return _settings;
                }

                var json = await File.ReadAllTextAsync(_file);
                _settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new AppSettings();
            }
            catch
            {
                _settings = new AppSettings(); // fallback
            }

            return _settings;
        }

        public async Task SaveAsync(AppSettings settings = null)
        {
            if (settings != null) _settings = settings;
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
            var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_file, json);
        }

        // Helpers para obtener/guardar secciones rápidamente
        public async Task<UnitsSettings> GetUnitsAsync() { var s = await LoadAsync(); return s.Units; }
        public async Task SaveUnitsAsync(UnitsSettings u) { var s = await LoadAsync(); s.Units = u; await SaveAsync(s); }

        public async Task<AlertThresholds> GetAlertsAsync() { var s = await LoadAsync(); return s.Alerts; }
        public async Task SaveAlertsAsync(AlertThresholds a) { var s = await LoadAsync(); s.Alerts = a; await SaveAsync(s); }

        public async Task<AppearanceSettings> GetAppearanceAsync() { var s = await LoadAsync(); return s.Appearance; }
        public async Task SaveAppearanceAsync(AppearanceSettings ap) { var s = await LoadAsync(); s.Appearance = ap; await SaveAsync(s); }
    }
}
