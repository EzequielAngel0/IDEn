using IDEn.Core.Enums;

namespace IDEn.Core.Models
{
    public class ConsumoEnergia
    {
        public int Anio { get; set; }
        public Mes Mes { get; set; }                 // enum
        public double KwhTotal { get; set; }
        public double KwhTransformador2 { get; set; }
        public double GasNaturalGJ { get; set; }
    }
}
