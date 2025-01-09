using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
