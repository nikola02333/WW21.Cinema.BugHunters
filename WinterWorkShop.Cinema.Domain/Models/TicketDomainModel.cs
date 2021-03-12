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

<<<<<<< Updated upstream
=======
        public Guid SeatId { get; set; }

>>>>>>> Stashed changes
        public SeatDomainModel Seat { get; set; }

        public Guid UserId { get; set; }

        public UserDomainModel User { get; set; }

        public Guid ProjectionId { get; set; }

        public ProjectionDomainModel Projection { get; set; }

    }
}
