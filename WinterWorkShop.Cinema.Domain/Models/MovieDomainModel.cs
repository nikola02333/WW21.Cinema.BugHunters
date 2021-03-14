﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieDomainModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool Current { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }

        public string? ActorName { get; set; }
    }
}
