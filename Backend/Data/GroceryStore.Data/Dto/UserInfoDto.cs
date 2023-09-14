using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.Dto
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool isAdmin { get; set; }
    }
}
