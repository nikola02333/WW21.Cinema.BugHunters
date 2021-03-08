using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

       
        public Guid SeatId { get; set; }

        public Seat Seat { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

      
    }
}
