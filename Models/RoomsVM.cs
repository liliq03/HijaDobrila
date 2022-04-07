using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Models
{
    public class RoomsVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public int RoomNum { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public int IdRoomType { get; set; }
        public List<SelectListItem> RoomType { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public string Images { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceForOneNight { get; set; }

    }
}

