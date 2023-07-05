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
using REPOs;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http;
using OldCarShowroomNetworkRazorPages.Api;
using OldCarShowroomNetworkRazorPages.Pagination;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{

    public class DetailsModel : PageModel
    {
        public readonly CarRepository _carRepo;
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
	
		public readonly BookingRepository _bookRepo;
		public readonly UserRepository _userkRepo;
		public DetailsModel(CarRepository carRepo, OldCarShowroomNetworkContext context,UserRepository userkRepo)
        {
            _carRepo = carRepo;
            _context =  new OldCarShowroomNetworkContext();
            _userkRepo = userkRepo;
		}

        public BOs.Models.Showroom Showroom { get; set; }
        public BOs.Models.ImageShowroom ImageShowroom { get; set; }
        public IList<BOs.Models.ImageShowroom> ImageShowrooms { get; set; }
        public PaginatedList<BOs.Models.Car> car { get; set; }
		public BOs.Models.User user { get; set; }
		public string email { get; set; }
		public async Task<IActionResult> OnGetAsync(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showroom = await _context.Showrooms
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.WardsNavigation).FirstOrDefaultAsync(m => m.ShowroomId == id);
            ImageShowroom = await _context.ImageShowrooms
               .FirstOrDefaultAsync(img => img.ShowroomId == id && img.ImageMain == true);

            ImageShowrooms = await _context.ImageShowrooms
                .Where(img => img.ShowroomId == id && img.ImageMain == false)
            .ToListAsync();

            var pageSize = 8;
            var list = from c in _context.Cars
                 .Include(c => c.ImageCars)
                 .Include(c => c.CarModelYearNavigation)
                 .Include(c => c.CarNameNavigation)
                 .Include(c => c.ColorInsideNavigation)
                 .Include(c => c.ColorNavigation)
                 .Include(c => c.DriveNavigation)
                 .Include(c => c.FuelNavigation)
                 .Include(c => c.ManufactoryNavigation)
                 .Include(c => c.Showroom).ThenInclude(s => s.City)
                 .Include(c => c.UsernameNavigation)
                 .Include(c => c.VehiclesNavigation).Where(c => c.ShowroomId == id && c.Notification == 1)
                 .OrderByDescending(c => c.AcceptedAt)
                 select c;
            car  = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);

            if (HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {
                email = HttpContext.Session.GetString("Key");
				user = await _userkRepo.GetAll().FirstOrDefaultAsync(u => u.Email == email);

                list = from c in _carRepo.GetAll()
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
                    .Include(c => c.Showroom.City)
                    .Include(c => c.VehiclesNavigation)
                    .Include(c => c.Showroom.City)
                    .Include(c => c.ImageCars)
                    .Include(c => c.Showroom.District)
                    .Include(c => c.Showroom.WardsNavigation)
                    .Where(c => c.Notification.Equals(1) && c.Username != user.Username && c.ShowroomId == id)
                    .OrderByDescending(c => c.AcceptedAt)
                    select c;
                car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);

                return Page();
			}
			if (Showroom == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
