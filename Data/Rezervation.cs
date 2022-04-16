using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class Rezervation
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Reservation")]
        public int RoomId { get; set; }
        public Room Rooms { get; set; }

        public string UserId { get; set; }
        public User Users { get; set; }

        public int AdultsNum { get; set; }
        public int ChildrensNum { get; set; }

        public DateTime DateArrived { get; set; }
        public DateTime DateLeft { get; set; }
        public DateTime DateRezervation { get; set; }
    }
}
