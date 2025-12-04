using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models.Dtos
{
    public class ProductDto
    {
        [Key]
        public string ProductID { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}
