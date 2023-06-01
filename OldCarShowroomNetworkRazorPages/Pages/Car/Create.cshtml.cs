using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CreateModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        
        public IActionResult OnGet()
        {
        ViewData["CarModelYear"] = new SelectList(_context.CarModelYears, "CarModelYearId", "CarModelYear1");
        ViewData["CarName"] = new SelectList(_context.CarNames, "CarNameId", "CarName1");
        ViewData["ColorInside"] = new SelectList(_context.Colors, "ColorId", "ColorName");
        ViewData["Color"] = new SelectList(_context.Colors, "ColorId", "ColorName");
        ViewData["Drive"] = new SelectList(_context.Drives, "DriveId", "DriveName");
        ViewData["Fuel"] = new SelectList(_context.Fuels, "FuelId", "FuelName");
        ViewData["ImageCar"] = new SelectList(_context.ImageCars, "ImageId", "ImageId");
        ViewData["Manufactory"] = new SelectList(_context.Manufactorys, "ManufactoryId", "ManufactoryName");
        ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
        ViewData["Vehicles"] = new SelectList(_context.Vehicles, "VehiclesId", "VehiclesName");
            return Page();
        }

        [BindProperty]
        public BOs.Models.Car Car { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
