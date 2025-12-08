using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Models.Dtos
{
    public class OrderDetailDto
    {
        [Key]
        public string OrderID { get; set; } = string.Empty;
        public int ProductID { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
