using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public BookingModel(CarRepository carRepo, BookingRepository bookingRepo, SlotRepository slotRepo, UserRepository userRepo)
        {
            _carRepo = carRepo;
            _bookingRepo = bookingRepo;
            _slotRepo = slotRepo;
            _userRepo = userRepo;
            _context = new OldCarShowroomNetworkContext();
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
        public TimeSpan checkTimeNow { get; set; }
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
                .Where(b => b.Username != null && b.Notification.Equals(1)).ToListAsync();

            Slot = await _slotRepo.GetAll().ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int carId, string userName, DateTime dateTime)
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

            DateTimeNow = DateTime.Now.Date;
            checkTimeNow = DateTime.Now.TimeOfDay;
            if (DateTimeNow > dateTime) {
                Msg = "Chỉ được đặt lịch từ hôm nay trở đi";
                return Page();
            }

            isBooked.CarId = carId;
            isBooked.Username = userName;
            isBooked.Notification = 1;
            isBooked.DayBooking = dateTime.Date;

            var checkBookAlready = await _bookingRepo.GetAll().Include(b => b.SlotNavigation).FirstOrDefaultAsync(b => b.Username == isBooked.Username && b.CarId == isBooked.CarId && b.Slot == isBooked.Slot && b.Notification.Equals(1));
            if(checkBookAlready != null){
                Msg3 = "Xe này đã đặt lịch rồi. Mời đặt lịch xem xe khác";
                return Page();
            }
            var checkUserBooking = await _bookingRepo.GetAll().FirstOrDefaultAsync(b => b.Username == isBooked.Username && b.DayBooking == isBooked.DayBooking && b.Notification.Equals(1));
            if(checkUserBooking != null) 
            {
                Msg2 = "Chỉ được đặt lịch xem xe trong showroom này 1 lần trong ngày";
                return Page();
            }

            var checkBooking = await _bookingRepo.GetAll().FirstOrDefaultAsync(b => b.DayBooking == isBooked.DayBooking && b.Slot == isBooked.Slot && b.Notification.Equals(1));
            if (checkUserBooking != null)
            {
                Msg3 = "Lịch đã có người đặt. Mời đặt lại";
                return Page();
            }

            _bookingRepo.Add(isBooked);

            var checkTime = await _bookingRepo.GetAll().Include(b => b.SlotNavigation).FirstOrDefaultAsync(b => b.DayBooking == isBooked.DayBooking && b.Slot == isBooked.Slot && b.Notification.Equals(1));
            if (DateTimeNow == checkTime.DayBooking && checkTimeNow > checkTime.SlotNavigation.PickupDate) {
                Msg1 = "Chỉ được đặt lịch từ giờ hiện tại trở đi";
                _bookingRepo.Delete(isBooked);
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
