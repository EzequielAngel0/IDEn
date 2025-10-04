using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDEn.Core.Enums;           // Mes
using IDEn.Core.Models;          // ProduccionMensual / ConsumoEnergia (no los tocamos)
using IDEn.Infrastructure.Data;  // IdenDbContext
using Microsoft.EntityFrameworkCore;

namespace IDEn.App.ViewModels
{
    /// <summary>
    /// ViewModel de la página "Datos".
    /// Contiene validación (INotifyDataErrorInfo) y comando Guardar (async).
    /// </summary>
    public class DatosViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        // ------------------ Campos / Propiedades ------------------
        private Mes? _mes;
        private int? _anio;

        private int? _inyectables;
        private int? _enterales;
        private int? _esterilizacion;

        private double? _kwhTotal;
        private double? _kwhT2;
        private double? _gasGJ;

        public Array Meses { get; } = Enum.GetValues(typeof(Mes));

        public Mes? Mes
        {
            get => _mes; set { _mes = value; Validate(); OnPropertyChanged(); }
        }

        public int? Anio
        {
            get => _anio; set { _anio = value; Validate(); OnPropertyChanged(); }
        }

        // Producción
        public int? SolucionesInyectables
        {
            get => _inyectables; set { _inyectables = value; Validate(); OnPropertyChanged(); }
        }
        public int? LiquidosEnterales
        {
            get => _enterales; set { _enterales = value; Validate(); OnPropertyChanged(); }
        }
        public int? CiclosEsterilizacion
        {
            get => _esterilizacion; set { _esterilizacion = value; Validate(); OnPropertyChanged(); }
        }

        // Energía
        public double? KwhTotal
        {
            get => _kwhTotal; set { _kwhTotal = value; Validate(); OnPropertyChanged(); }
        }
        public double? KwhT2
        {
            get => _kwhT2; set { _kwhT2 = value; Validate(); OnPropertyChanged(); }
        }
        public double? GasGJ
        {
            get => _gasGJ; set { _gasGJ = value; Validate(); OnPropertyChanged(); }
        }

        // Comando
        public ICommand GuardarCommand { get; }

        public DatosViewModel()
        {
            Anio = DateTime.Now.Year;
            GuardarCommand = new AsyncRelayCommand(GuardarAsync, () => !HasErrors);
        }

        // ------------------ Validación ------------------
        private readonly Dictionary<string, List<string>> _errors = new();
        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
            => propertyName != null && _errors.TryGetValue(propertyName, out var list) ? list : null;

        void AddError(string prop, string message)
        {
            if (!_errors.ContainsKey(prop)) _errors[prop] = new List<string>();
            if (!_errors[prop].Contains(message)) _errors[prop].Add(message);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop));
        }

        void ClearErrors(string prop)
        {
            if (_errors.Remove(prop))
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop));
        }

        void Validate()
        {
            // Mes
            ClearErrors(nameof(Mes));
            if (Mes is null) AddError(nameof(Mes), "Seleccione el mes.");

            // Año
            ClearErrors(nameof(Anio));
            if (Anio is null) AddError(nameof(Anio), "Ingrese el año.");
            else if (Anio < 2000 || Anio > 2100) AddError(nameof(Anio), "Año fuera de rango (2000-2100).");

            // Producción
            ValidateNonNeg(nameof(SolucionesInyectables), SolucionesInyectables);
            ValidateNonNeg(nameof(LiquidosEnterales), LiquidosEnterales);
            ValidateNonNeg(nameof(CiclosEsterilizacion), CiclosEsterilizacion);

            // Energía
            ValidateNonNeg(nameof(KwhTotal), KwhTotal);
            ValidateNonNeg(nameof(KwhT2), KwhT2);
            ValidateNonNeg(nameof(GasGJ), GasGJ);

            // Refresca CanExecute del botón
            CommandManager.InvalidateRequerySuggested();
        }

        static void ValidateNonNeg(string prop, int? v, DatosViewModel vm)
        {
            vm.ClearErrors(prop);
            if (v is null) vm.AddError(prop, "Campo requerido.");
            else if (v < 0) vm.AddError(prop, "No puede ser negativo.");
        }
        void ValidateNonNeg(string prop, int? v) => ValidateNonNeg(prop, v, this);

        static void ValidateNonNeg(string prop, double? v, DatosViewModel vm)
        {
            vm.ClearErrors(prop);
            if (v is null) vm.AddError(prop, "Campo requerido.");
            else if (v < 0) vm.AddError(prop, "No puede ser negativo.");
        }
        void ValidateNonNeg(string prop, double? v) => ValidateNonNeg(prop, v, this);

        // ------------------ Guardar (EF Core) ------------------
        private async Task GuardarAsync()
        {
            // seguridad extra
            Validate();
            if (HasErrors) return;

            try
            {
                using var db = new IdenDbContext();

                // PRODUCCIÓN: upsert por (Anio, Mes)
                var prod = await db.Producciones.FindAsync(Anio!.Value, Mes!.Value);
                if (prod is null)
                {
                    prod = new ProduccionMensual
                    {
                        Anio = Anio.Value,
                        Mes = Mes.Value,
                        SolucionesInyectables = SolucionesInyectables!.Value,
                        LiquidosEnterales = LiquidosEnterales!.Value,
                        CiclosEsterilizacion = CiclosEsterilizacion!.Value
                    };
                    db.Producciones.Add(prod);
                }
                else
                {
                    prod.SolucionesInyectables = SolucionesInyectables!.Value;
                    prod.LiquidosEnterales = LiquidosEnterales!.Value;
                    prod.CiclosEsterilizacion = CiclosEsterilizacion!.Value;
                    db.Producciones.Update(prod);
                }

                // CONSUMO ENERGÍA: upsert por (Anio, Mes)
                var con = await db.Consumos.FindAsync(Anio!.Value, Mes!.Value);
                if (con is null)
                {
                    con = new ConsumoEnergia
                    {
                        Anio = Anio.Value,
                        Mes = Mes.Value,
                        KwhTotal = KwhTotal!.Value,
                        KwhTransformador2 = KwhT2!.Value,
                        GasNaturalGJ = GasGJ!.Value
                    };
                    db.Consumos.Add(con);
                }
                else
                {
                    con.KwhTotal = KwhTotal!.Value;
                    con.KwhTransformador2 = KwhT2!.Value;
                    con.GasNaturalGJ = GasGJ!.Value;
                    db.Consumos.Update(con);
                }

                await db.SaveChangesAsync();
                MessageBox.Show("Datos guardados con éxito.", "IDEn", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "IDEn", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Comando asíncrono sin colisión con tu RelayCommand existente.
    /// </summary>
    public sealed class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _exec;
        private readonly Func<bool> _can;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _exec = execute ?? throw new ArgumentNullException(nameof(execute));
            _can = canExecute;
        }

        public bool CanExecute(object parameter) => _can?.Invoke() ?? true;

        public async void Execute(object parameter) => await _exec();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
