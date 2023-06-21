using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class CarDetailModel : PageModel
    {
        public readonly CarRepository _carRepo;
        public BOs.Models.Car car { get; set; }
        public CarDetailModel(CarRepository carRepo)
        {
            _carRepo = carRepo;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("Key") == null || HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {   

                if (_carRepo.GetAll() != null)
                {
                    car = await _carRepo.GetAll()
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
                        .Include(c => c.VehiclesNavigation)
                        .Include(c => c.Showroom.City)
                        .Include(c => c.Showroom.District)
                        .Include(c => c.Showroom.WardsNavigation).FirstOrDefaultAsync(m => m.CarId == id);
                }
                if (car == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
    }
}
