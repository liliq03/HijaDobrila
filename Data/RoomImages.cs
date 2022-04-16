using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class RoomImages
    {
        public RoomImages()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        //wrazka M:1
        [Required]
        public int RoomId { get; set; }
        public Room Room
        {
            get; set;
        }
    }
}
