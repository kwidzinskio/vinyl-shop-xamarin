using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class UserImageModel
    {
        public string ImageUrl { get; set; }
        public string FullImagePath => $"http://192.168.1.9:45455/{ImageUrl}";
    }
}
