using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieDomainModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool Current { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }
        public bool HasOscar { get; set; }
        public string CoverPicture { get; set; }
        public string Description { get; set; }
        public string? Imdb { get; set; }


        public string[] Tags { get; set; }

        public string[] Actors { get; set; }
    }
}
