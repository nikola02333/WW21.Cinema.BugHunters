using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class ImdbMovieReturn
    {
        public int Year { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public int CoverPicture { get; set; }

        public string[] Actors { get; set; }

    }
}

/*
   Title: string;
      Year: number;
      Current: boolean;
      Rating: number;
      Tags: string;
      CoverPicture: string;
      Genre: string;
      Actors: string;
      Description:string;
      ImdbId?: string;
 */
