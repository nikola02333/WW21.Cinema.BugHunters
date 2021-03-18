using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
  public  class TagsMovies
    {
        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
