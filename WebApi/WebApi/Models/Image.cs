using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ItemId { get; set; }
        [NotMapped]
        public byte[] ImageArray { get; set; }
    }
}
