using System.Collections.Generic;
using System.Linq;

namespace KhanhHoaTravel.Models.ViewModels
{
    public class PlaceListViewModel
    {
        public IEnumerable<EntertainmentPlace> tblPlaceDeltail { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentGenre { get; set; }
    }
}
