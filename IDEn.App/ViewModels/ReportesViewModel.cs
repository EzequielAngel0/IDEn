using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDEn.Core.Enums;              // Mes
using IDEn.Infrastructure.Data;     // IdenDbContext
using IDEn.App.Utils;               // AsyncRelayCommand
using Microsoft.EntityFrameworkCore;

namespace IDEn.App.ViewModels
{
    public enum ReportType { ProduccionYEnergia, SoloProduccion, SoloEnergia }
    public enum ExportFormat { Csv, Excel, Pdf }

    public class ReportRow
    {
        public string Periodo { get; set; }           // AAAA-MM
        public int? Inyectables { get; set; }
        public int? Enterales { get; set; }
        public int? Esterilizacion { get; set; }
        public double? KwhTotal { get; set; }
        public double? KwhT2 { get; set; }
        public double? GasGJ { get; set; }
    }

    public sealed class ReportesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        // Listas para los ComboBox (evita x:Static en XAML)
        public Array ReportTypes { get; } = Enum.GetValues(typeof(ReportType));
        public Array ExportFormats { get; } = Enum.GetValues(typeof(ExportFormat));

        private ReportType? _selectedReportType;
        public ReportType? SelectedReportType
        {
            get => _selectedReportType;
            set { _selectedReportType = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private ExportFormat? _selectedFormat;
        public ExportFormat? SelectedFormat
        {
            get => _selectedFormat;
            set { _selectedFormat = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private DateTime? _startDate = new DateTime(DateTime.Now.Year, 1, 1);
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private DateTime? _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public ObservableCollection<ReportRow> Rows { get; } = new();

        public ICommand GenerarCommand { get; }
        public ICommand ExportarCommand { get; }

        public ReportesViewModel()
        {
            GenerarCommand = new AsyncRelayCommand(GenerarAsync, PuedeGenerar);
            ExportarCommand = new AsyncRelayCommand(ExportarAsync, PuedeExportar);
        }

        bool PuedeGenerar() =>
            SelectedReportType.HasValue && StartDate.HasValue && EndDate.HasValue && StartDate <= EndDate;

        bool PuedeExportar() =>
            Rows.Count > 0 && SelectedFormat.HasValue;

        // ---------------- Generación ----------------
        async Task GenerarAsync()
        {
            if (!PuedeGenerar()) return;

            try
            {
                Rows.Clear();

                var from = new DateTime(StartDate!.Value.Year, StartDate.Value.Month, 1);
                var to = new DateTime(EndDate!.Value.Year, EndDate.Value.Month, 1);

                int keyFrom = from.Year * 100 + from.Month;
                int keyTo = to.Year * 100 + to.Month;

                using var db = new IdenDbContext();

                var prods = await db.Producciones.AsNoTracking()
                    .Where(p => (p.Anio * 100 + (int)p.Mes) >= keyFrom &&
                                (p.Anio * 100 + (int)p.Mes) <= keyTo)
                    .ToListAsync();

                var cons = await db.Consumos.AsNoTracking()
                    .Where(c => (c.Anio * 100 + (int)c.Mes) >= keyFrom &&
                                (c.Anio * 100 + (int)c.Mes) <= keyTo)
                    .ToListAsync();

                var cursor = from;
                while (cursor <= to)
                {
                    int y = cursor.Year;
                    var m = (Mes)cursor.Month;

                    var prod = prods.FirstOrDefault(p => p.Anio == y && p.Mes == m);
                    var con = cons.FirstOrDefault(c => c.Anio == y && c.Mes == m);

                    var row = new ReportRow
                    {
                        Periodo = $"{y}-{cursor.Month:00}",
                        Inyectables = prod?.SolucionesInyectables,
                        Enterales = prod?.LiquidosEnterales,
                        Esterilizacion = prod?.CiclosEsterilizacion,
                        KwhTotal = con?.KwhTotal,
                        KwhT2 = con?.KwhTransformador2,
                        GasGJ = con?.GasNaturalGJ
                    };

                    switch (SelectedReportType)
                    {
                        case ReportType.SoloProduccion:
                            row.KwhTotal = row.KwhT2 = row.GasGJ = null; break;
                        case ReportType.SoloEnergia:
                            row.Inyectables = row.Enterales = row.Esterilizacion = null; break;
                    }

                    Rows.Add(row);
                    cursor = cursor.AddMonths(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message, "IDEn",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------------- Exportación ----------------
        async Task ExportarAsync()
        {
            if (!PuedeExportar()) return;

            try
            {
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string baseName = $"Reporte_IDEn_{DateTime.Now:yyyyMMdd_HHmmss}";

                switch (SelectedFormat)
                {
                    case ExportFormat.Csv:
                    case ExportFormat.Excel: // Excel abrirá el CSV sin problemas
                        {
                            string path = Path.Combine(docs, $"{baseName}.csv");
                            await File.WriteAllTextAsync(path, ToCsv(Rows), Encoding.UTF8);
                            MessageBox.Show($"Archivo generado:\n{path}", "IDEn",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        }
                    case ExportFormat.Pdf:
                        {
                            string path = Path.Combine(docs, $"{baseName}.html");
                            await File.WriteAllTextAsync(path, ToHtml(Rows), Encoding.UTF8);
                            MessageBox.Show($"Archivo HTML generado (Imprimir → Guardar como PDF):\n{path}", "IDEn",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = path,
                                UseShellExecute = true
                            });
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "IDEn",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static string ToCsv(ObservableCollection<ReportRow> rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Periodo,Inyectables,Enterales,Esterilizacion,KwhTotal,KwhT2,GasGJ");
            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(",",
                    r.Periodo,
                    r.Inyectables?.ToString() ?? "",
                    r.Enterales?.ToString() ?? "",
                    r.Esterilizacion?.ToString() ?? "",
                    r.KwhTotal?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
                    r.KwhT2?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
                    r.GasGJ?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? ""
                ));
            }
            return sb.ToString();
        }

        static string ToHtml(ObservableCollection<ReportRow> rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!doctype html><html><head><meta charset='utf-8'><title>Reporte IDEn</title>");
            sb.AppendLine("<style>body{font-family:Segoe UI, Arial; margin:24px;} table{border-collapse:collapse;width:100%;} th,td{border:1px solid #e5e7eb;padding:6px 8px;text-align:right;} th{text-align:center;background:#f3f4f6} td:first-child, th:first-child{text-align:left}</style>");
            sb.AppendLine("</head><body><h2>Reporte IDEn</h2><table><thead><tr>");
            string[] headers = { "Periodo", "Inyectables", "Enterales", "Esterilizacion", "KwhTotal", "KwhT2", "GasGJ" };
            foreach (var h in headers) sb.Append("<th>").Append(h).Append("</th>");
            sb.AppendLine("</tr></thead><tbody>");
            foreach (var r in rows)
            {
                sb.Append("<tr>")
                  .Append("<td>").Append(r.Periodo).Append("</td>")
                  .Append("<td>").Append(r.Inyectables?.ToString() ?? "").Append("</td>")
                  .Append("<td>").Append(r.Enterales?.ToString() ?? "").Append("</td>")
                  .Append("<td>").Append(r.Esterilizacion?.ToString() ?? "").Append("</td>")
                  .Append("<td>").Append(r.KwhTotal?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "").Append("</td>")
                  .Append("<td>").Append(r.KwhT2?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "").Append("</td>")
                  .Append("<td>").Append(r.GasGJ?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "").Append("</td>")
                  .AppendLine("</tr>");
            }
            sb.AppendLine("</tbody></table></body></html>");
            return sb.ToString();
        }
    }
}
