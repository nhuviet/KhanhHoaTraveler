using System.Linq;

namespace KhanhHoaTravel.Models
{
    public interface IKHTravelRepository
    {
        IQueryable<EntertainmentPlace> tblPlaceDeltail { get; }
    }
}

