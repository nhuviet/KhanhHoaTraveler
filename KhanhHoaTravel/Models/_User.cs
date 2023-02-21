using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Models
{
    public class _User
    {
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
		public string DateCreate { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string FaceBook { get; set; }
		public string Website { get; set; }
		public string Image { get; set; }
		public int Status = 0;
	}
}
