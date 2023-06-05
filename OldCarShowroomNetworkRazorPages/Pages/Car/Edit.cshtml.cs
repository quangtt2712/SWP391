using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BOs.Models;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    public class EditModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public EditModel()
        {
            _context = new OldCarShowroomNetworkContext();
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
                .Include(c => c.ImageCarNavigation)
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation).FirstOrDefaultAsync(m => m.CarId == id);

            if (Car == null)
            {
                return NotFound();
            }
            ViewData["CarModelYear"] = new SelectList(_context.CarModelYears, "CarModelYearId", "CarModelYear1");
            ViewData["CarName"] = new SelectList(_context.CarNames, "CarNameId", "CarName1");
            ViewData["ColorInside"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["Color"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["Drive"] = new SelectList(_context.Drives, "DriveId", "DriveName");
            ViewData["Fuel"] = new SelectList(_context.Fuels, "FuelId", "FuelName");
            ViewData["ImageCar"] = new SelectList(_context.ImageCars, "ImageId", "ImageId");
            ViewData["Manufactory"] = new SelectList(_context.Manufactorys, "ManufactoryId", "ManufactoryName");
            ViewData["Username"] = new SelectList(_context.Users, "Username", "FullName");
            ViewData["Vehicles"] = new SelectList(_context.Vehicles, "VehiclesId", "VehiclesName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(Car.CarId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
