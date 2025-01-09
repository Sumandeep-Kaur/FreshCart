using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
