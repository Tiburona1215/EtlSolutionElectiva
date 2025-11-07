using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimProducto
    {
        public int ProductKey { get; set; }
        public string Codigo_Producto { get; set; } = string.Empty;
        public string Nombre_Producto { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio_Act { get; set; }
        public decimal Costo { get; set; }
        public string Unidad_Medida { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;

        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
