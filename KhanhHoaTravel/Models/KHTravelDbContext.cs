using Microsoft.EntityFrameworkCore;

namespace KhanhHoaTravel.Models
{
    public class KHTravelDbContext : DbContext
    {
        public KHTravelDbContext(DbContextOptions<KHTravelDbContext> options) : base(options) { }
        public DbSet<EntertainmentPlace> tblPlaceDeltail { get; set; }
    }
}

