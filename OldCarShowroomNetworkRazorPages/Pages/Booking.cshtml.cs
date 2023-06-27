using AspNetCoreHero.ToastNotification.Abstractions;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using REPOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages
{
    [Authorize(Roles = "User")]
    public class BookingModel : PageModel
    {
        public readonly CarRepository _carRepo;
        public readonly BookingRepository _bookingRepo;
        public readonly SlotRepository _slotRepo;
        public readonly UserRepository _userRepo;
        public readonly OldCarShowroomNetworkContext _context;
        private readonly INotyfService _toastNotification;

        public BookingModel(CarRepository carRepo, BookingRepository bookingRepo, SlotRepository slotRepo, UserRepository userRepo, OldCarShowroomNetworkContext context, INotyfService toastNotification)
        {
            _carRepo = carRepo;
            _bookingRepo = bookingRepo;
            _slotRepo = slotRepo;
            _userRepo = userRepo;
            _context = new OldCarShowroomNetworkContext();
            _toastNotification = toastNotification;
        }

        public BOs.Models.Car Car { get; set; }
        public BOs.Models.User user { get; set; }
        public IList<BOs.Models.Slot> Slot { get; set; }
        [BindProperty]
        public BOs.Models.Slot SlotBooked { get; set; }
        public IList<BOs.Models.Booking> booking { get; set; }
        [BindProperty]
        public BOs.Models.Booking isBooked { get; set; }
        public string Email { get; set; }
        [BindProperty]
        public string Msg { get; set; }
        [BindProperty]
        public string Msg1 { get; set; }
        [BindProperty]
        public string Msg2 { get; set; }
        [BindProperty]
        public string Msg3 { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime DateTimeNow { get; set; }
        public DateTime checkTimeNow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Slot"] = new SelectList(_context.Slots, "SlotId", "PickupDate");
            dateTime = DateTime.Now;
            Email = HttpContext.Session.GetString("Key");
            user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
            if(id == null) 
            { 
                return NotFound();
            }
            Car =  await _context.Cars
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
                .FirstOrDefaultAsync(c => c.CarId == id);

            booking = await _bookingRepo.GetAll()
                .Include(s => s.SlotNavigation)
                .Where(b => b.Username != user.Username && b.Notification.Equals(1)).ToListAsync();

            Slot = await _slotRepo.GetAll().ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int carId, string userName, DateTime dateTime)
        {
            Email = HttpContext.Session.GetString("Key");
            user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
            ViewData["Slot"] = new SelectList(_context.Slots, "SlotId", "PickupDate");
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

            DateTimeNow = DateTime.Now.Date;
            checkTimeNow = DateTime.Now;
            if (DateTimeNow > dateTime) {
                Msg = "Chỉ được đặt lịch từ ngày " + DateTimeNow.ToString("dd/mm/yyyy") + "trở đi";
                _toastNotification.Error("Đặt lịch xem xe thất bại");
                return Page();
            }

            isBooked.CarId = carId;
            isBooked.Username = userName;
            isBooked.Notification = 1;
            isBooked.DayBooking = dateTime.Date;
            isBooked.Note = "";
            var checkUserAlreadyBooking = await _bookingRepo.GetAll().FirstOrDefaultAsync(b => b.CarId == carId && b.Username == userName && b.Notification.Equals(2));
            if (checkUserAlreadyBooking != null) {
                checkUserAlreadyBooking.Notification = 1;
                checkUserAlreadyBooking.DayBooking = dateTime.Date;
                checkUserAlreadyBooking.Slot = isBooked.Slot;
                checkUserAlreadyBooking.Note = "";
                _bookingRepo.Update(checkUserAlreadyBooking);
                _toastNotification.Success("Đặt laị lịch xem xe thành công");
                return RedirectToPage("/Index");
            }
            var checkBooking = await _bookingRepo.GetAll().FirstOrDefaultAsync(b => b.DayBooking == isBooked.DayBooking && b.Slot == isBooked.Slot && b.Notification.Equals(1));
            if (checkBooking != null)
            {
                Msg3 = "Lịch đã có người đặt. Mời đặt lại";
                _toastNotification.Error("Đặt lịch xem xe thất bại");
                return Page();
            }
            _bookingRepo.Add(isBooked);

            var checkTime = await _bookingRepo.GetAll().Include(b => b.SlotNavigation).FirstOrDefaultAsync(b => b.DayBooking == isBooked.DayBooking && b.Slot == isBooked.Slot && b.Notification.Equals(1));
            if (isBooked.Slot == 4 && DateTimeNow == isBooked.DayBooking && checkTimeNow.TimeOfDay > isBooked.SlotNavigation.PickupDate)
            {
                Msg1 = "Hết thời gian xem xe hôm nay. Mời đặt lịch từ ngày mai trở đi";
                _toastNotification.Error("Đặt lịch xem xe thất bại");
                _bookingRepo.Delete(isBooked);
                return Page();
            }

            if (DateTimeNow == isBooked.DayBooking && checkTimeNow.TimeOfDay > isBooked.SlotNavigation.PickupDate)
            {
                Msg1 = "Chỉ được đặt lịch từ " + checkTimeNow.ToString("HH:mm") + " trở đi";
                _toastNotification.Error("Đặt lịch xem xe thất bại");
                _bookingRepo.Delete(isBooked);
                return Page();
            }

            _toastNotification.Success("Đặt lịch xem xe thành công");
            return RedirectToPage("/Index");
        }
    }
}
