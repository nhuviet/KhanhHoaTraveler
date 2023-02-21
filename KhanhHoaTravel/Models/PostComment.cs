using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Models
{
    public class PostComment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public _User User { get; set; }
        public String Content { get; set; }
        public string TimeModified { get; set; }
        public int status { get; set; }
    }
}
