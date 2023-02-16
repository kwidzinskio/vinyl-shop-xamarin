using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class SearchItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public object Album { get; set; }
        public object Band { get; set; }
    }
}
