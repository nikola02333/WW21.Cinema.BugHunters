using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Domain.Models
{
   public class TagDomainModel
    {
        public int TagId { get; set; }
        public string TagName { get; set; }

        public string? TagValue { get; set; }

        public List<TagsMovies> TagsMovies { get; set; }
    }
}
