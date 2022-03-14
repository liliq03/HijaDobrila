using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class RoomType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public ICollection<Rezervation> Rezervations { get; set; }
    }
}
