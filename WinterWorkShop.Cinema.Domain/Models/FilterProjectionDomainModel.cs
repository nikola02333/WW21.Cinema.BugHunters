using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class FilterProjectionDomainModel
    {
        public int? CinemaId { get; set; } = null;
        public int? AuditoriumId { get; set; } = null;
        public Guid? MovieId { get; set; } = null;
        public DateTime? DateTime { get; set; } = null;
    }
}
