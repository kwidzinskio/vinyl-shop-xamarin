using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace WebApi.Models
{
    public class Item
    {
        
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Album { get; set; }

        public string ReleaseYear { get; set; }

        public string Band { get; set; }

        public DateTime DatePosted { get; set; }

        public bool IsHotAndNew { get; set; }

        public bool IsFeatured { get; set; }

        public string Location { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Condition { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}
