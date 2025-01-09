using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [RegularExpression("^(Pending|Processing|Shipped|Delivered|Cancelled)$")]
        public string Status { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
