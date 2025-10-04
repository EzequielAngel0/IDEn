using IDEn.App.Commands;

namespace IDEn.App.ViewModels
{
    public class ReportesViewModel : ViewModelBase
    {
        private string _periodo = "";   // ← inicializado (evita CS8618)
        public string Periodo
        {
            get => _periodo;
            set { _periodo = value; OnPropertyChanged(); }
        }

        public AsyncRelayCommand GenerarReporteCmd { get; }

        public ReportesViewModel()
        {
            GenerarReporteCmd = new AsyncRelayCommand(GenerarReporteAsync, () => true);
        }

        private async Task GenerarReporteAsync()
        {
            await Task.CompletedTask;
        }
    }
}
