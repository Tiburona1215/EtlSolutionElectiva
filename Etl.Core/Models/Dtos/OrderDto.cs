using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models.Dtos
{
    public class OrderDto
    {
        [Key]
        public string OrderID { get; set; } = string.Empty;
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ShipCity { get; set; } = string.Empty;
        public string ShipCountry { get; set; } = string.Empty;
        public string ShipRegion { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string StoreID { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
    }
}
