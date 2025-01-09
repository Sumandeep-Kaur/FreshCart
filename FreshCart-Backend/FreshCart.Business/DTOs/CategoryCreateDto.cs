using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
