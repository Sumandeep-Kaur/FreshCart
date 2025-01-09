using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
}
