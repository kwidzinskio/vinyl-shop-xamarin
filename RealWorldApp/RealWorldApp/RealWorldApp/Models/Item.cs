using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Item
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Album { get; set; }

        public string ReleaseYear { get; set; }

        public string Band { get; set; }

        public DateTime DatePosted { get; set; }

        public string Location { get; set; }

        public string Condition { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

    }
}