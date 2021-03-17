using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class SeatsMaxNumbersDomainModel
    {
        public int AuditoriumId { get; set; }

        public int MaxRow { get; set; }

        public int MaxNumber { get; set; }
    }
}
