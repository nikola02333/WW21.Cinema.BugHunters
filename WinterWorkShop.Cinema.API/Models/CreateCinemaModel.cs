using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateCinemaModel
    {

        [Required]
        [StringLength(50, ErrorMessage = Messages.CINEMA_PROPERTIE_NAME_REQUIERED)]
        public string Name { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = Messages.CINEMA_PROPERTIE_ADRESS_REQUIERED)]
        public string Address { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = Messages.CINEMA_PROPERTIE_CITY_NAME_REQUIERED)]
        public string CityName { get; set; }

        public List<CreateAuditoriumModel?> createAuditoriumModel  { get; set; }
    }
}
