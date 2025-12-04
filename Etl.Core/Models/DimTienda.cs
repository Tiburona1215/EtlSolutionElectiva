using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimTienda
    {
        [Key]
        public int TiendaKey { get; set; }
        public string Nombre_Tienda { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Tipo_Tienda { get; set; } = string.Empty;

        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
