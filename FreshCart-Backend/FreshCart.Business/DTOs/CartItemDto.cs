using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
