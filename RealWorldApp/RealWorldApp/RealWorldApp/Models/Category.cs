using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public object Items { get; set; }
    }
}
