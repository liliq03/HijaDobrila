using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int RoomNum { get; set; }

        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceForOneNight { get; set; }
        public ICollection<Rezervation> Rezervations { get; set; }
    }
}
