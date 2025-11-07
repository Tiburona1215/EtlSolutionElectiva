using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimCanal
    {
        public int CanalKey { get; set; }
        public string Nombre_Canal { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
