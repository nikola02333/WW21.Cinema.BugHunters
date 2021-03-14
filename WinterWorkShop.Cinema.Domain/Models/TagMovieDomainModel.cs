using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
   public class TagMovieDomainModel
    {
        public MovieDomainModel MovieModel { get; set; }

        public TagDomainModel TagModel { get; set; }
    }
}
