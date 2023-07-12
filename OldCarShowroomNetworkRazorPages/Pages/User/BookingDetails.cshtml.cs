using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User")]
    public class BookingDetailsModel : PageModel
	{
		public readonly BookingRepository _bookingRepo;
		public readonly UserRepository _userRepo;

		public BookingDetailsModel(BookingRepository bookingRepo, UserRepository userRepo)
		{
			_bookingRepo = bookingRepo;
			_userRepo = userRepo;
		}

		public BOs.Models.Booking booking { get; set; }
		public BOs.Models.User user { get; set; }
		public string Email { get; set; }
		public string Msg { get; set; }
		public async Task<IActionResult> OnGetAsync(int? carId)
		{
			Email = HttpContext.Session.GetString("Key");
			user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
			booking = await _bookingRepo.GetAll()
					.Include(b => b.UsernameNavigation)
					.Include(b => b.Car)
					.Include(b => b.Car.ManufactoryNavigation)
					.Include(b => b.Car.CarNameNavigation)
					.Include(b => b.Car.CarModelYearNavigation)
					.Include(b => b.Car.UsernameNavigation)
					.Include(b => b.Car.VehiclesNavigation)
					.Include(b => b.Car.ColorInsideNavigation)
					.Include(b => b.Car.ColorNavigation)
					.Include(b => b.Car.DriveNavigation)
					.Include(b => b.Car.FuelNavigation)
					.Include(b => b.Car.Showroom)
					.Include(b => b.Car.Showroom.City)
					.Include(b => b.Car.Showroom.District)
					.Include(b => b.Car.Showroom.WardsNavigation)
					.Include(b => b.SlotNavigation)
					.FirstOrDefaultAsync(b => b.CarId == carId && b.Username.Equals(user.Username));

            if (booking == null)
			{
				Msg = "Không thể xem lịch";
				return Page();
			}

			return Page();
		}
	}
}
