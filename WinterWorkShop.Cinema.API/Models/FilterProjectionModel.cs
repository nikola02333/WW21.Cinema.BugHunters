using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class FilterProjectionModel
    {
        [FromQuery]
        public int? CinemaId { get; set; } = null;
        [FromQuery]
        public int? AuditoriumId { get; set; } = null;
        [FromQuery]
        public Guid? MovieId { get; set; } = null;
        [FromQuery]
        public DateTime? DateTime { get; set; } = null;
    }
}
