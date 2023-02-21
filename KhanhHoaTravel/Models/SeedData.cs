using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace KhanhHoaTravel.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            KHTravelDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<KHTravelDbContext>();
            //if (context.Database.GetPendingMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}
            if (!context.tblPlaceDeltail.Any())
            {
                context.tblPlaceDeltail.AddRange(
                new EntertainmentPlace
                {
                    Title = "Vinpearl Land",
                    Author = "admin",
                    TimeOpen = "7h00",
                    TimeClose = "20h00",
                    Rate = 4.5,
                    Genre = "Vui chơi, giải trí",
                    Address = "Cầu đá",
                    Description = "",
                    ImagePath = "1.jpg",
                    Status = 1
                });
                context.SaveChanges();
            }
        }
    }
}
