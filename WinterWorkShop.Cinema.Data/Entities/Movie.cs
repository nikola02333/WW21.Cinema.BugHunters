using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("movie")]
    public class Movie
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public bool HasOscar { get; set; }

        public double? Rating { get; set; }

        public bool Current { get; set; }

        public float? UserRaitings { get; set; }

        public string    Genre { get; set; }
        public string? CoverPicture { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public ICollection<TagsMovies> TagsMovies { get; set; }
    }
}
