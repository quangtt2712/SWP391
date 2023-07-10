using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OldCarShowroomNetworkRazorPages.Pagination;
using REPOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User")]
    public class ListBookingModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public readonly BookingRepository _bookingRepo;

        public ListBookingModel(UserRepository userRepo, BookingRepository bookingRepo)
        {
            _userRepo = userRepo;
            _bookingRepo = bookingRepo;
        }

        public PaginatedList<BOs.Models.Booking> booking { get; set; }
        public BOs.Models.User user { get; set; }
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            Email = HttpContext.Session.GetString("Key");
            user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
            var pageSize = 4;
            var list = from b in _bookingRepo.GetAll()
                .Include(b => b.Car)
                .Include(b => b.Car.ManufactoryNavigation)
                .Include(b => b.Car.CarNameNavigation)
                .Include(b => b.Car.CarModelYearNavigation)
                .Include(b => b.Car.Showroom)
                .Include(b => b.Car.Showroom.City)
                .Include(b => b.Car.Showroom.District)
                .Include(b => b.Car.Showroom.WardsNavigation)
                .Include(b => b.SlotNavigation)
                .Where(b => b.Username.Equals(user.Username) && b.Notification == 1)
                .OrderByDescending(b => b.DayBooking)
                .ThenByDescending(b => b.SlotNavigation.PickupDate)
                select b;

            booking = await PaginatedList<BOs.Models.Booking>.CreateAsync(list, pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
