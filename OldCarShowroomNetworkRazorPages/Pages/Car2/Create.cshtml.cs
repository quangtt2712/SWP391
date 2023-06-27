using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;

namespace OldCarShowroomNetworkRazorPages.Pages.Car2
{
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CreateModel(BOs.Models.OldCarShowroomNetworkContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CarModelYear"] = new SelectList(_context.CarModelYears, "CarModelYearId", "CarModelYearId");
        ViewData["CarName"] = new SelectList(_context.CarNames, "CarNameId", "CarNameId");
        ViewData["ColorInside"] = new SelectList(_context.Colors, "ColorId", "ColorId");
        ViewData["Color"] = new SelectList(_context.Colors, "ColorId", "ColorId");
        ViewData["Drive"] = new SelectList(_context.Drives, "DriveId", "DriveId");
        ViewData["Fuel"] = new SelectList(_context.Fuels, "FuelId", "FuelId");
        ViewData["Manufactory"] = new SelectList(_context.Manufactorys, "ManufactoryId", "ManufactoryId");
        ViewData["ShowroomId"] = new SelectList(_context.Showrooms, "ShowroomId", "Phone");
        ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
        ViewData["Vehicles"] = new SelectList(_context.Vehicles, "VehiclesId", "VehiclesId");
            return Page();
        }

        /*[BindProperty]
        public Car Car { get; set; }

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
        }*/
    }
}
