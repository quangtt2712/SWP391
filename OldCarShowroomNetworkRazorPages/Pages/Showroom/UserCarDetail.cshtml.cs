using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
	[Authorize(Roles = "Staff")]
	public class UserCarDetailModel : PageModel
	{
		public readonly CarRepository _carRepo;
		public readonly BookingRepository _bookRepo;
		public readonly UserRepository _userRepo;
		private readonly INotyfService _toastNotification;

		public UserCarDetailModel(CarRepository carRepo, BookingRepository bookRepo, UserRepository userRepo, INotyfService toastNotification)
		{
			_carRepo = carRepo;
			_bookRepo = bookRepo;
			_userRepo = userRepo;
			_toastNotification = toastNotification;
		}

		public BOs.Models.Car car { get; set; }
		public BOs.Models.User user { get; set; }
		public string email { get; set; }
		public string Msg { get; set; }
		public string Msg1 { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
				if (_carRepo.GetAll() != null)
				{
					car = await _carRepo.GetAll()
						.Include(c => c.ImageCars)
						.Include(c => c.CarModelYearNavigation)
						.Include(c => c.CarNameNavigation)
						.Include(c => c.ColorInsideNavigation)
						.Include(c => c.ColorNavigation)
						.Include(c => c.DriveNavigation)
						.Include(c => c.FuelNavigation)
						.Include(c => c.ManufactoryNavigation)
						.Include(c => c.Showroom)
						.Include(c => c.UsernameNavigation)
						.Include(c => c.VehiclesNavigation)
						.Include(c => c.Showroom.City)
						.Include(c => c.Showroom.District)
						.Include(c => c.Showroom.WardsNavigation).FirstOrDefaultAsync(m => m.CarId == id);
					if (car.Notification == 3)
					{
						Msg1 = "Xe đã được bán rồi";
						return Page();
					}
					if (car.Notification == 2)
					{
						Msg1 = "Xe đã bị showroom từ chối kí gửi";
						return Page();
					}
			}
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(int? CarId)
		{
			car = await _carRepo.GetAll()
			.Include(c => c.ImageCars)
			.Include(c => c.CarModelYearNavigation)
			.Include(c => c.CarNameNavigation)
			.Include(c => c.ColorInsideNavigation)
			.Include(c => c.ColorNavigation)
			.Include(c => c.DriveNavigation)
			.Include(c => c.FuelNavigation)
			.Include(c => c.ManufactoryNavigation)
			.Include(c => c.Showroom)
			.Include(c => c.UsernameNavigation)
			.Include(c => c.VehiclesNavigation)
			.Include(c => c.Showroom.City)
			.Include(c => c.Showroom.District)
			.Include(c => c.Showroom.WardsNavigation).FirstOrDefaultAsync(m => m.CarId == CarId);
			var checkNotifi = _carRepo.GetAll().FirstOrDefault(c => c.CarId == CarId);
			checkNotifi.Notification = 1;
			checkNotifi.AcceptedAt = DateTime.Now;
			_carRepo.Update(checkNotifi);
			_toastNotification.Success("Chấp nhận kí gửi xe thành công");
			return RedirectToPage("./ListCar");
		}
	}
}
