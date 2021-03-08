using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
   public class MovieDetails
    {
        public float UserRatings { get; set; }

        public List<string> Pictures { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
