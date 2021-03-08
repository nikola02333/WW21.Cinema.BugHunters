﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("auditorium")]
    public class Auditorium
    {
        public int Id { get; set; }

        [Column("cinemaId")]
        public int CinemaId { get; set; }

        [Column("AuditoriumName")]
        public string AuditName { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public virtual Cinema Cinema { get; set; }
        
    }
}
