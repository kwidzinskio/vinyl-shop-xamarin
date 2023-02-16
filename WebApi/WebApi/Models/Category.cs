using System.Collections.Generic;

namespace WebApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
