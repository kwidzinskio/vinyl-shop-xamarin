using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ItemByCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Album { get; set; }
        public string Location { get; set; }
        public object Band { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsFeatured { get; set; }
        public string ImageUrl { get; set; }

        public string IsFeaturedAd => IsFeatured ? "Featured" : "Free";

        public string FullImageUrl => $"http://192.168.1.9:45455/{ImageUrl}";
        public string AdPostedDate => DatePosted.ToString("y");
    }
}
