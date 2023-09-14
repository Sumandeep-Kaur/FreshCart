using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Data.EntityModels
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new Collection<CartItem>();
    }
}
