using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public Guid SeatId { get; set; }

        public Seat Seat { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public Guid ProjectionId { get; set; }

        public Projection Projection { get; set; }

    }
}
