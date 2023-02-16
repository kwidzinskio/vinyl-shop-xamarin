using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ItemDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Album { get; set; }
        public string ReleaseYear { get; set; }
        public string Band { get; set; }
        public DateTime DatePosted { get; set; }
        public string Condition { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Image> Images { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => $"http://192.168.1.9:45455/{ImageUrl}";
    }

    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ItemId { get; set; }
        public object ImageArray { get; set; }
        public string FullImageUrl => $"http://192.168.1.9:45455/{ImageUrl}";
    }
}
