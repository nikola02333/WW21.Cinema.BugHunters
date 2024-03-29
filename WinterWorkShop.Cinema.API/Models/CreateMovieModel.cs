﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateMovieModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.MOVIE_PROPERTIE_TITLE_NOT_VALID)]
        public string Title { get; set; }
        
        [Required]
        [Range(1895, 2100, ErrorMessage = Messages.MOVIE_PROPERTIE_YEAR_NOT_VALID)]
        public int Year{ get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = Messages.MOVIE_PROPERTIE_RATING_NOT_VALID)]
        public double Rating { get; set; }

        [Required]
        [StringLength(20)]
        public string Genre { get; set; }
        public bool Current { get; set; }

        public bool HasOscar { get; set; }
        public double UserRaitings { get; set; }
        public string CoverPicture { get; set; }

        public string Tags { get; set; }

        public string Actors { get; set; }

        public string? ImdbId { get; set; }



        public string Description { get; set; }

    }
}
