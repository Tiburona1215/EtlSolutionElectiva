using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class RawVentaDto
    {
        public int FechaKey { get; set; }
        public int ProductKey { get; set; }
        public int ClienteKey { get; set; }
        public int TiendaKey { get; set; }
        public int CanalKey { get; set; }

        //vent
        public int Cantidad_Vendida { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total_Venta { get; set; }
        public decimal Costo { get; set; }
        public decimal Ganancia { get; set; }

        //others
        public string Moneda { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;
    }
}
