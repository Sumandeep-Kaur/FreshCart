using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshCart.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Range(0, 100)]
        [Precision(5, 2)]
        public decimal? DiscountPercentage { get; set; } = 0;

        [Range(1, 5)]
        public int AverageRating { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
