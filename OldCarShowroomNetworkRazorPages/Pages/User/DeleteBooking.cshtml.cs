using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User")]
    public class DeleteBookingModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public readonly BookingRepository _bookingRepo;

        public DeleteBookingModel(UserRepository userRepo, BookingRepository bookingRepo)
        {
            _userRepo = userRepo;
            _bookingRepo = bookingRepo;
        }
        public BOs.Models.Booking booking { get; set; }
        public async Task<IActionResult> OnGetAsync(string UserName, DateTime datetime, int carId)
        {
            booking = await _bookingRepo.GetAll()
                .Include(b => b.Car)
                .Include(b => b.Car.ManufactoryNavigation)
                .Include(b => b.Car.CarNameNavigation)
                .Include(b => b.Car.CarModelYearNavigation)
                .Include(b => b.Car.Showroom)
                .Include(b => b.Car.Showroom.City)
                .Include(b => b.Car.Showroom.District)
                .Include(b => b.Car.Showroom.WardsNavigation)
                .Include(b => b.SlotNavigation)
                .FirstOrDefaultAsync(b => b.Username.Equals(UserName) && b.DayBooking.Equals(datetime) && b.CarId == carId && b.Notification.Equals(1));
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string UserName, DateTime datetime)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var deleteBooking = _bookingRepo.GetAll().FirstOrDefault(b => b.Username.Equals(UserName) && b.DayBooking.Equals(datetime) && b.Notification.Equals(1));
            if (deleteBooking == null)
            {
                return Page();
            }
            _bookingRepo.Delete(deleteBooking);
            return RedirectToPage("./ListBooking");
        }
    }
}
