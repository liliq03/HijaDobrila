using HijaDobrila2.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Models
{
    public class RezervationsVM
    {
        public int Id { get; set; }

      //  [Required(ErrorMessage = "This field is  required ")]
        public int RoomId { get; set; }

        public List<SelectListItem> Rooms { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public int AdultsNum { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public int ChildrensNum { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата на вписване")]


        public DateTime DateArrived { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата на напускане")]
        public DateTime DateLeft { get; set; }


        [Display(Name = "Дата на резервация")]
        public DateTime DateRezervation { get; set; }

    }
}

