using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    [Table("Ticket")]
    public class Ticket
    {
        public Guid Id { get; set; }

        public int price { get; set; }

        public DateTime Created { get; set; }

        public Guid SeatId { get; set; }

        public Seat Seat { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid ProjectionId { get; set; }

        public Projection Projection { get; set; }

    }
}
