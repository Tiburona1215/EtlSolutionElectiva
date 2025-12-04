using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimCliente
    {
        [Key]
        public int ClienteKey { get; set; }
        public int ID_Cliente { get; set; }
        public string Nombre_Cliente { get; set; } = string.Empty;
        public string Tipo_Cliente { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Codigo_Postal { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime Fecha_Registro { get; set; }

        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
