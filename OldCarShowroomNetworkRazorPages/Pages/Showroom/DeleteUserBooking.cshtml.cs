using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class DeleteUserBookingModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public readonly BookingRepository _bookingRepo;
        private readonly INotyfService _toastNotification;

        public DeleteUserBookingModel(UserRepository userRepo, BookingRepository bookingRepo, INotyfService toastNotification)
        {
            _userRepo = userRepo;
            _bookingRepo = bookingRepo;
            _toastNotification = toastNotification;
        }

        public BOs.Models.Booking booking { get; set; }
        [BindProperty]
        public string Note { get; set; }
        public string Msg { get; set; }
        public string Msg1 { get; set; }
        public async Task<IActionResult> OnGetAsync(string UserName, int? carId)
        {
            booking = await _bookingRepo.GetAll()
                .Include(b => b.UsernameNavigation)
                .Include(b => b.Car)
                .Include(b => b.Car.ManufactoryNavigation)
                .Include(b => b.Car.CarNameNavigation)
                .Include(b => b.Car.CarModelYearNavigation)
                .Include(b => b.Car.Showroom)
                .Include(b => b.SlotNavigation)
                .FirstOrDefaultAsync(b => b.Username.Equals(UserName) && b.CarId == carId && b.Notification.Equals(1));

            if(booking == null)
            {
                Msg1 = "Lịch đã bị xóa hoặc không có";
                return Page();
            }
            if (booking.Notification == 2)
            {
                Msg1 = "Đã xóa lịch đặt rồi";
                return Page();
            }
            if (booking.Notification == 3)
            {
                Msg1 = "Xe đã bán rồi. Không thể xóa lịch";
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string UserName, int? carId)
        {
			booking = await _bookingRepo.GetAll()
	        .Include(b => b.UsernameNavigation)
	        .Include(b => b.Car)
	        .Include(b => b.Car.ManufactoryNavigation)
	        .Include(b => b.Car.CarNameNavigation)
	        .Include(b => b.Car.CarModelYearNavigation)
	        .Include(b => b.Car.Showroom)
	        .Include(b => b.SlotNavigation)
	        .FirstOrDefaultAsync(b => b.Username.Equals(UserName) && b.CarId == carId && b.Notification.Equals(1));
			if (!ModelState.IsValid)
            {
                _toastNotification.Error("Xoá lịch khách xem xe thất bại");
                return Page();
            }
            if (Note == null)
            {
                Msg = "Cần nhập lý do để từ chối kí gửi xe";
                _toastNotification.Error("Xoá lịch khách xem xe thất bại");
                return Page();
            }
            var deleteBooking = _bookingRepo.GetAll().FirstOrDefault(b => b.Username.Equals(UserName) && b.CarId == carId && b.Notification.Equals(1));
            if (deleteBooking != null)
            {
                deleteBooking.Note = Note;
                deleteBooking.Notification = 2;
                _bookingRepo.Update(deleteBooking);
                _toastNotification.Success("Xoá lịch khách xem xe thành công");
                return RedirectToPage("./ListUserBooking");
            }
            return Page();
        }
    }
}
