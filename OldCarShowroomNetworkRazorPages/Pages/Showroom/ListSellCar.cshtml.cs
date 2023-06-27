using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class ListSellCarModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public readonly BookingRepository _bookingRepo;

        public ListSellCarModel(UserRepository userRepo, BookingRepository bookingRepo)
        {
            _userRepo = userRepo;
            _bookingRepo = bookingRepo;
        }

        public IList<BOs.Models.Booking> booking { get; set; }
        [BindProperty]
        public BOs.Models.User user { get; set; }
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            booking = await _bookingRepo.GetAll()
                .Include(b => b.UsernameNavigation)
                .Include(b => b.Car)
                .Include(b => b.Car.UsernameNavigation)
                .Include(b => b.Car.ManufactoryNavigation)
                .Include(b => b.Car.CarNameNavigation)
                .Include(b => b.Car.CarModelYearNavigation)
                .Include(b => b.Car.Showroom)
                .Include(b => b.Car.Showroom.City)
                .Include(b => b.Car.Showroom.District)
                .Include(b => b.Car.Showroom.WardsNavigation)
                .Include(b => b.SlotNavigation)
                .Where(b => b.Notification == 3)
                .ToListAsync();
            return Page();
        }
    }
}
