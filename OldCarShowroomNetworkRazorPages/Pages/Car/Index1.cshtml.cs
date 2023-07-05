using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using OldCarShowroomNetworkRazorPages.Pagination;
using REPOs;
using System.Xml.Schema;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class IndexModel1: PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        public readonly BookingRepository _bookingRepo;

        public IndexModel1(OldCarShowroomNetworkContext context, BookingRepository bookingRepo)
        {
            _context = new OldCarShowroomNetworkContext();
            _bookingRepo = bookingRepo;
        }

        public PaginatedList<BOs.Models.Car> Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
			string userLogin = HttpContext.Session.GetString("Key");
			var user = _context.Users.FirstOrDefault(s => s.Email.Equals(userLogin));
			var pageSize = 8;
			var list = from c in _context.Cars
				 .Include(c => c.Bookings)
				 .Include(c => c.ImageCars)
				 .Include(c => c.CarModelYearNavigation)
				 .Include(c => c.CarNameNavigation)
				 .Include(c => c.ColorInsideNavigation)
				 .Include(c => c.ColorNavigation)
				 .Include(c => c.DriveNavigation)
				 .Include(c => c.FuelNavigation)
				 .Include(c => c.ManufactoryNavigation)
				 .Include(c => c.Showroom)
				 .ThenInclude(s => s.City)
				 .Include(c => c.UsernameNavigation)
				 .Include(c => c.VehiclesNavigation)
				 .Where(s => s.Username.Equals(user.Username) && s.Notification == 1)
				 .OrderByDescending(c => c.CreatedAt)
				 select c;

			Car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);
			return Page();
        }
    }
}
