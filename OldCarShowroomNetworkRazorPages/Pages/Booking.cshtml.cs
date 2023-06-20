using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public BookingModel(CarRepository carRepo, BookingRepository bookingRepo, SlotRepository slotRepo, UserRepository userRepo)
        {
            _carRepo = carRepo;
            _bookingRepo = bookingRepo;
            _slotRepo = slotRepo;
            _userRepo = userRepo;
        }

        public BOs.Models.Car Car { get; set; }
        public BOs.Models.User User { get; set; }
        public IList<BOs.Models.Slot> Slot { get; set; }
        public BOs.Models.Slot SlotBooked { get; set; }
        public IList<BOs.Models.Booking> Booking { get; set; }
        [BindProperty]
        public BOs.Models.Booking isBooked { get; set; }
        public string Email { get; set; }
        [BindProperty]
        public string Msg { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime DateTimeNow { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            dateTime = DateTime.Now.Date;
            DateTimeNow = DateTime.Now.Date;
            Email = HttpContext.Session.GetString("Key");
            User = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
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
                .Where(c => c.CarId == id)
                .FirstOrDefaultAsync();
            if (Car == null)
            {
                return NotFound();
            }

            Booking = await _bookingRepo.GetAll()
                .Include(s => s.SlotNavigation)
                .Where(s => s.Username != null).ToListAsync();

            Slot = await _slotRepo.GetAll().ToListAsync();
            foreach (var slot in Slot)
            {
                foreach (var book in Booking)
                {
                    if (slot.SlotId != book.Slot)
                        slot.IsBooked = false;
                    else
                        slot.IsBooked = true;
                }
            }
            return Page();
        }
        public IActionResult OnPost(int SlotId)
        {

            if (DateTimeNow > dateTime) {
                Msg = "Chỉ được đặt lịch từ hôm nay trở đi";
                return Page();
            }
            //SlotBooked = Slot.FirstOrDefault(s => s.SlotId ==);
            isBooked.CarId = Car.CarId;
            isBooked.Username = User.Username;
            isBooked.Slot = SlotId;
            isBooked.Notification = 1;
            _bookingRepo.Add(isBooked);
            return RedirectToPage("/Index");
        }
    }
}
