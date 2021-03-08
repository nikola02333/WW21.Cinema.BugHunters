using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateUserModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_USERNAME_REQUIRED)]

        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_NAME_REQUIRED)]

        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.USER_LASTNAME_REQUIRED)]
        public string LastName { get; set; }

        public string Role { get; set; }

    }
}
