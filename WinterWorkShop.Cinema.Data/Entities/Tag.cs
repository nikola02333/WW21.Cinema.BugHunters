using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
   public class Tag
    {
        public string TagId { get; set; }

        public string? TagValue { get; set; }

        public ICollection<TagsMovies> TagsMovies { get; set; }
    }
}
