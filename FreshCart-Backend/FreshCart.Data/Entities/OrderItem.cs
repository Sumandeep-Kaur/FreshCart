using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        [Range(0, 100)]
        [Precision(5, 2)]
        public decimal? DiscountPercentage { get; set; }
    }
}
