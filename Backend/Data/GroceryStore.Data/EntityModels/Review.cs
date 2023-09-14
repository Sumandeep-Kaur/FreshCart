using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.EntityModels
{
    public class Review
    {
        public int Id { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }
        public int ProductId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReviewDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
