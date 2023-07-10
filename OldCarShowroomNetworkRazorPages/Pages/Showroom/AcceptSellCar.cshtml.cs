using AspNetCoreHero.ToastNotification.Abstractions;
using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using REPOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class AcceptSellCarModel : PageModel
    {
        public readonly CarRepository _carRepo;
        public readonly BookingRepository _bookingRepo;
        public readonly SlotRepository _slotRepo;
        public readonly UserRepository _userRepo;
        public readonly OldCarShowroomNetworkContext _context;
        private readonly INotyfService _toastNotification;

        public AcceptSellCarModel(CarRepository carRepo, BookingRepository bookingRepo, SlotRepository slotRepo, UserRepository userRepo, OldCarShowroomNetworkContext context, INotyfService toastNotification)
        {
            _carRepo = carRepo;
            _bookingRepo = bookingRepo;
            _slotRepo = slotRepo;
            _userRepo = userRepo;
            _context = new OldCarShowroomNetworkContext();
            _toastNotification = toastNotification;
        }

        public BOs.Models.Car Car { get; set; }
        public BOs.Models.Booking Booking { get; set; }
        public IList<BOs.Models.Booking> listBooking { get; set; }
        public string Msg { get; set; }
		public string Msg1 { get; set; }
		public string Msg2 { get; set; }
		[BindProperty]
        public int Money { get; set; }
        public DateTime dateTime { get; set; }  
        public async Task<IActionResult> OnGetAsync(int? carId, string Username)
        {
            Car = await _context.Cars
                .Include(c => c.ImageCars)
                .Include(c => c.CarModelYearNavigation)
                .Include(c => c.CarNameNavigation)
                .Include(c => c.ColorInsideNavigation)
                .Include(c => c.ColorNavigation)
                .Include(c => c.DriveNavigation)
                .Include(c => c.FuelNavigation)
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.Showroom)
                .Include(c => c.Showroom.City)
                .Include(c => c.Showroom.District)
                .Include(c => c.Showroom.WardsNavigation)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation)
                .FirstOrDefaultAsync(c => c.CarId == carId);

            Booking = await _bookingRepo.GetAll()
                .Include(s => s.UsernameNavigation)
                .Include(s => s.SlotNavigation).FirstOrDefaultAsync(b => b.Username == Username && b.Notification.Equals(1) && b.CarId == carId);
            if (Car.Notification == 3)
            {
                Msg1 = "Xe đã được bán rồi";
                return Page();
            }
			if (Car.Notification == 2)
			{
				Msg1 = "Xe bị từ chối kí gửi bởi showroom. Không thể bán xe";
				return Page();
			}
			return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? CarId, string Username)
        {
            dateTime = DateTime.Now;

            Car = await _carRepo.GetAll()
                .Include(c => c.ImageCars)
                .Include(c => c.CarModelYearNavigation)
                .Include(c => c.CarNameNavigation)
                .Include(c => c.ColorInsideNavigation)
                .Include(c => c.ColorNavigation)
                .Include(c => c.DriveNavigation)
                .Include(c => c.FuelNavigation)
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.Showroom)
                .Include(c => c.Showroom.City)
                .Include(c => c.Showroom.District)
                .Include(c => c.Showroom.WardsNavigation)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation)
                .FirstOrDefaultAsync(c => c.CarId == CarId);

            Booking = await _bookingRepo.GetAll()
                .Include(s => s.UsernameNavigation)
                .Include(s => s.SlotNavigation)
                .FirstOrDefaultAsync(b => b.Username == Username && b.Notification.Equals(1) && b.CarId == CarId);

            listBooking = await _bookingRepo.GetAll()
                .Include(s => s.UsernameNavigation)
                .Include(s => s.SlotNavigation)
                .Where(b => b.Notification.Equals(1) && b.CarId == CarId)
                .ToListAsync();

            if (Money == 0)
            {
                Msg = "Cần nhập số tiền đã bán để xác nhận bán xe";
                _toastNotification.Error("Xác nhận bán xe thất bại");
                return Page();
            }

            if (Money <= 0)
            {
                Msg = "Cần nhập số nhập số dương";
                _toastNotification.Error("Xác nhận bán xe thất bại");
                return Page();
            }

            if (Money < Car.MinPrice)
            {
                Msg = "Cần nhập số cao hơn giá tối thiểu";
                _toastNotification.Error("Xác nhận bán xe thất bại");
                return Page();
            }

            if (dateTime.Date < Booking.DayBooking)
            {
                Msg2 = "Không được xác nhận bán xe trước ngày khách đã đặt lịch";
                _toastNotification.Error("Xác nhận bán xe thất bại");
                return Page();
            }

            if (dateTime.Date == Booking.DayBooking && dateTime.TimeOfDay < Booking.SlotNavigation.PickupDate)
            {
                Msg2 = "Không được xác nhận bán xe trước giờ khách đã đặt lịch";
                _toastNotification.Error("Xác nhận bán xe thất bại");
                return Page();
            }

            foreach(var item in listBooking)
            {
                if(Booking.SlotNavigation.PickupDate > item.SlotNavigation.PickupDate)
                {
                    Msg2 = "Không được xác nhận bán xe do còn lịch đặt của khách trước";
                    _toastNotification.Error("Xác nhận bán xe thất bại");
                    return Page();
                }
            }

            var checkAcceptSell = _carRepo.GetAll().FirstOrDefault(c => c.CarId == CarId);
            if (checkAcceptSell != null)
            {
                checkAcceptSell.Notification = 3;
                checkAcceptSell.Price = Money;
                _carRepo.Update(checkAcceptSell);
                Booking.Notification = 3;
                _bookingRepo.Update(Booking);
                var listBooking = await _bookingRepo.GetAll().Where(b => b.CarId == CarId && b.Username != Username && b.Notification.Equals(1)).ToListAsync();
                foreach(var item in listBooking)
                {
                    item.Notification = 2;
                    item.Note = "Hủy vì xe đã bán rồi";
                    _bookingRepo.Update(item);
                }
                _toastNotification.Success("Xác nhận bán xe thành công");
            }
            return RedirectToPage("./ListUserBooking");
        }
    }
}
