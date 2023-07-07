using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BOs.Models
{
	public partial class User
	{
		public User()
		{
			Bookings = new HashSet<Booking>();
			Cars = new HashSet<Car>();
		}

		[Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
		[StringLength(126, ErrorMessage = "Tên người dùng không được vượt quá 126 kí tự.")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Tên người dùng là bắt buộc.")]

		[StringLength(50, ErrorMessage = "Tên không được vượt quá 50 kí tự.")]
		public string FullName { get; set; }
		[Required(ErrorMessage = "Địa chỉ là bắt buộc.")]

		[StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 kí tự.")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Email là bắt buộc.")]
		[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email không hợp lệ.")]
		[StringLength(99, ErrorMessage = "Email không được vượt quá 99 kí tự.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
		[StringLength(16, ErrorMessage = "Mật khẩu không được vượt quá 16 kí tự.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
		[RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Số điện thoại không hợp lệ.")]
		[StringLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 kí tự.")]
		public string Phone { get; set; }

		public int? RoleId { get; set; }

		public virtual Role Role { get; set; }
		public virtual ICollection<Booking> Bookings { get; set; }
		public virtual ICollection<Car> Cars { get; set; }
	}
}
