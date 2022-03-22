using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class Rooms
    {
        public int Id { get; set; }
        public int RoomNum { get; set; }
        public int IdRoomType { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceForOneNight { get; set; }
        public ICollection<Rezervation> Rezervations { get; set; }
    }
}
