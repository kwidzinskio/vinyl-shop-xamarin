using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ItemMap
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Album { get; set; }
        public string Location { get; set; }
        public string Band { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsFeatured { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public object ImageUrl { get; set; }
    }
}
