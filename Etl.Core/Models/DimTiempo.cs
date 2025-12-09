using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimTiempo
    {
        [Key]
        public int FechaKey { get; set; }
        public DateTime Fecha_Complet { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Trimestre { get; set; }
        public int Día { get; set; }
        public string? Nombre_Mes { get; set; } = string.Empty;
        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
