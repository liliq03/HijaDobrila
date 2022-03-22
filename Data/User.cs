using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HijaDobrila2.Data
{
    public class User : IdentityUser
    {
    
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Telephone{ get; set; }
        //public Roles Role { get; set; }

        public ICollection<Rezervation> Rezervations { get; set; }
    }
}
