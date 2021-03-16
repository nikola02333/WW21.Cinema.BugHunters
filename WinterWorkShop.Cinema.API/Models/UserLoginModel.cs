using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_NAME_REQUIRED)]
        public string UserName { get; set; }

    }
}
