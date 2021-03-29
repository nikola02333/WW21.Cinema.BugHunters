using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDbApiLib;
using IMDbApiLib.Models;

namespace WinterWorkShop.Cinema.API.Models
{
    public class IMDBModel
    {
        public TitleData Data { get; set; }
        public YouTubeTrailerData YoutubeData { get; set; }
    }
}
