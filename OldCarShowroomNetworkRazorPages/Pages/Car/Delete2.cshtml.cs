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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class DeleteModel2 : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        private readonly INotyfService _toastNotification;

        public DeleteModel2(OldCarShowroomNetworkContext context, INotyfService toastNotification)
        {
            _context = new OldCarShowroomNetworkContext();
            _toastNotification = toastNotification;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car = await _context.Cars.FindAsync(id);

            if (Car != null)
            {
                var associatedImages = await _context.ImageCars
            .Where(img => img.CarId == Car.CarId)
            .ToListAsync();


                _context.ImageCars.RemoveRange(associatedImages);
                await _context.SaveChangesAsync();
                _context.Cars.Remove(Car);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Xóa xe thành công");
            }
            return RedirectToPage("./Index");
        }
    }
}
