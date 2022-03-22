using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Models
{
    public class RoomTypesVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is  required ")]
        public string TypeName { get; set; }
    }
}
