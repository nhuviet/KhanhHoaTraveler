using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Models
{
    public class EntertainmentPlace
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string TimeOpen { get; set; }
        public string TimeClose { get; set; }
        public double Rate { get; set; }
        public string Address { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int Status { get; set; }
    }
}
