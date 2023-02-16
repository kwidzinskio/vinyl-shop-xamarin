using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class HotAndNewAd
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Album { get; set; }
        public string Band { get; set; }
        public bool IsFeatured { get; set; }
        public object ImageUrl { get; set; }
        public string FullImageUrl => $"http://192.168.1.9:45455/{ImageUrl}";
    }
}
