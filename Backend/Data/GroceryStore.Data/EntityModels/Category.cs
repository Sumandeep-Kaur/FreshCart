using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Data.EntityModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
    }
}
