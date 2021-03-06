using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HijaDobrila2.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public  DbSet<Rezervation> Rezervations { get; set; }
        public  DbSet<RoomType> RoomTypes { get; set; }
        public  DbSet<Room> Rooms { get; set; }

    }
}

