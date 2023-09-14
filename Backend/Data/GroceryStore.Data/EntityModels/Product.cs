using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryStore.Data.EntityModels
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Discount { get; set; }

        [Required]
        public int UnitsInStock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [MaxLength(100)]
        public string Specs { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<Review> Reviews { get; set; } = new Collection<Review>();
    }
}
