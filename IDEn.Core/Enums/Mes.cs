using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEn.Core.Enums
{
    public enum Mes
    {
        Ene = 1, Feb, Mar, Abr, May, Jun, Jul, Ago, Sep, Oct, Nov, Dic
    }

    public static class MesExtensions
    {
        private static readonly string[] NombresCortos =
            { "", "Ene","Feb","Mar","Abr","May","Jun","Jul","Ago","Sep","Oct","Nov","Dic" };

        public static string ToShortName(this Mes mes) => NombresCortos[(int)mes];
    }
}
