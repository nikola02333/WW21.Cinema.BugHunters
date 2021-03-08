﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("User")]
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        
        public string UserName { get; set; }        

        
        public string Role { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
