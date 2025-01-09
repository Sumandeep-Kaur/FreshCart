using System;
using System.ComponentModel.DataAnnotations;

namespace FreshCart.Business.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string ImageUrl { get; set; }
        public int AverageRating { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
