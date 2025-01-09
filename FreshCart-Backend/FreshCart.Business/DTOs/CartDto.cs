using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
