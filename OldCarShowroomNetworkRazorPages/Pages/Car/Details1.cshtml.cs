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

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class DetailsModel1 : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public DetailsModel1()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        public BOs.Models.Car Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car = await _context.Cars
                .Include(c => c.CarModelYearNavigation)
                .Include(c => c.CarNameNavigation)
                .Include(c => c.ColorInsideNavigation)
                .Include(c => c.ColorNavigation)
                .Include(c => c.DriveNavigation)
                .Include(c => c.FuelNavigation)
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.Showroom)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation).FirstOrDefaultAsync(m => m.CarId == id);

            if (Car == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
