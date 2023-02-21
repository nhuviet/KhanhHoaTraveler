using KhanhHoaTravel.Models;
using System.Linq;

namespace KhanhHoaTravel.Models
{
    public class EFKHTravelRepository : IKHTravelRepository
    {
        private KHTravelDbContext context;
        public EFKHTravelRepository(KHTravelDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<EntertainmentPlace> tblPlaceDeltail => context.tblPlaceDeltail;
    }

}