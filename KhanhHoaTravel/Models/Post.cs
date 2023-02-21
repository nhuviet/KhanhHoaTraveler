using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Models
{
    public class Post
    {
        public int Id { get; set; }
        public _User User { get; set; }
        public String Content { get; set; }
        public string TimeModified { get; set; }
        public string ImagePath { get; set; }
        public int LikeCount = 0;
        public int CommentCount = 0;
        public int status { get; set; }

    }
}
