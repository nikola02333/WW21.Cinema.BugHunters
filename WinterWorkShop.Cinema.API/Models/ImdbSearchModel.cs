using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDbApiLib.Models;

namespace WinterWorkShop.Cinema.API.Models
{
   public class Actor
    {
        //public int Id { get; set; }

        public string Name { get; set; }
    }
    public class ImdbSearchModel
    {
        public string Genres { get; set; }

        public string Year { get; set; }

        public string RuntimeMins { get; set; }

        public List<Actor> ActorList { get; set; }

        public string IMDbRating { get; set; }

        public string Title { get; set; }

        public string Plot { get; set; }
        public string Image { get; set; }

        public string Awards { get; set; }
    }
}
