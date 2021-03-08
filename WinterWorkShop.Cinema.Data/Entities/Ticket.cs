using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public int AuditoriumId { get; set; }

        public Auditorium Auditorium { get; set; }
        public Guid SeatId { get; set; }

        public Seat Seat { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public Guid MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
