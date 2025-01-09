using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Data.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
