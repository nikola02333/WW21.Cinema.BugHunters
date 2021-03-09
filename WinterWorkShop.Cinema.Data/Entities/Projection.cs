using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("projection")]
    public class Projection
    {
        public Guid Id { get; set; }

     
        public int AuditoriumId { get; set; }

        public virtual Auditorium Auditorium { get; set; }

        public DateTime ShowingDate { get; set; }

        public int Duration { get; set; }
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

    }
}
