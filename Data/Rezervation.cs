using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class Rezervation
    {
        public int Id { get; set; }
        public int IdRoom { get; set; }
        public Room Rooms { get; set; }

        public string IdUser { get; set; }
        public User Users { get; set; }

        public int AdultsNum { get; set; }
        public int ChildrensNum { get; set; }

        public DateTime DateArrived { get; set; }
        public DateTime DateLeft { get; set; }
        public DateTime DateRezervation { get; set; }
    }
}
