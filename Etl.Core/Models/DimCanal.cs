using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models
{
    public class DimCanal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CanalKey { get; set; }
        public string Descripción { get; set; } = string.Empty;
        public string Nombre_Canal { get; set; } = string.Empty;
        public ICollection<FactVent> Ventas { get; set; } = new List<FactVent>();
    }
}
