using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class TicketDomainModel
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public int Price { get; set; }

        public SeatDomainModel Seat { get; set; }

        public UserDomainModel User { get; set; }

        public ProjectionDomainModel Projection { get; set; }

    }
}
