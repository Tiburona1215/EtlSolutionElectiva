using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class FactVent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Venta { get; set; }
        public int FechaKey { get; set; }
        public int ProductKey { get; set; }
        public int ClienteKey { get; set; }
        public int Cantidad_Vendida { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total_Venta { get; set; }
        public decimal Costo { get; set; }
        public decimal Ganancia { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;

        public DimProducto? Producto { get; set; }
        public DimCliente? Cliente { get; set; }
        public DimTiempo? Tiempo { get; set; }
    }
}
