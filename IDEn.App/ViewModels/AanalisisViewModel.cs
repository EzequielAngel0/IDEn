using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;               // Point
using System.Windows.Media;         // PointCollection

namespace IDEn.App.ViewModels
{
    public class AnalisisViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        public AnalisisViewModel(string indicatorKey = null)
        {
            IndicatorKey = indicatorKey ?? "electricidad_total";
            Title = "Indicador de Desempeño Energético (IDEn)";
            Unit = "kWh/pza";
            AvailableYears = new[] { 2023, 2024, 2025 };
            SelectedYear = 2024;

            // Datos mock para demo (12 meses). Luego cargaremos desde DB.
            MonthlyValues = new ObservableCollection<double>(new double[]
            { 1.35, 1.22, 1.28, 1.10, 1.80, 1.40, 1.32, 1.25, 1.18, 1.15, 1.26, 1.33 });

            RecomputeKpis();
            UpdateChartPoints();
        }

        // --------- Props de presentación ----------
        public string IndicatorKey { get; }
        public string Title { get; private set; }
        public string Unit { get; private set; }

        private double _currentValue;
        public double CurrentValue { get => _currentValue; private set { _currentValue = value; OnPropertyChanged(); } }

        private double _growthPct;
        public double GrowthPct { get => _growthPct; private set { _growthPct = value; OnPropertyChanged(); } }

        public string[] Months { get; } = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

        public int[] AvailableYears { get; }
        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set { _selectedYear = value; OnPropertyChanged(); /*recargar si cambia*/ }
        }

        public ObservableCollection<double> MonthlyValues { get; }

        // Polilínea ya preparada para XAML
        private PointCollection _chartPoints;
        public PointCollection ChartPoints { get => _chartPoints; private set { _chartPoints = value; OnPropertyChanged(); } }

        public double ChartWidth { get; set; } = 720;
        public double ChartHeight { get; set; } = 220;

        void RecomputeKpis()
        {
            if (MonthlyValues.Count == 0) { CurrentValue = 0; GrowthPct = 0; return; }
            CurrentValue = MonthlyValues[Math.Min(DateTime.Now.Month, MonthlyValues.Count) - 1];
            var prev = Math.Max(0.0001, CurrentValue - 0.06); // mock para demo
            GrowthPct = (CurrentValue - prev) / prev;         // +/-
        }

        void UpdateChartPoints()
        {
            // Escala valores a alto del canvas
            double max = 0;
            foreach (var v in MonthlyValues) if (v > max) max = v;
            if (max <= 0) max = 1;

            var pts = new PointCollection();
            var n = MonthlyValues.Count;
            if (n == 0) { ChartPoints = pts; return; }

            double stepX = ChartWidth / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double x = i * stepX;
                double yNorm = MonthlyValues[i] / max;     // 0..1
                double y = ChartHeight - (yNorm * ChartHeight);
                pts.Add(new Point(x, y));
            }
            ChartPoints = pts;
        }
    }
}
