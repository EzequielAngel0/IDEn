using IDEn.Core.Enums;

namespace IDEn.Core.Models
{
    public class ProduccionMensual
    {
        public int Anio { get; set; }
        public Mes Mes { get; set; }                 // enum (ver punto 2)
        public int SolucionesInyectables { get; set; }
        public int LiquidosEnterales { get; set; }
        public int CiclosEsterilizacion { get; set; }
    }
}
