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
    public class RefuseCarModel : PageModel
    {
        public readonly CarRepository _carRepo;

        public RefuseCarModel(CarRepository carRepo)
        {
            _carRepo = carRepo;
        }
        [BindProperty]
        public string Note { get; set; }
        [BindProperty]
        public BOs.Models.Car car { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
                .Include(c => c.Showroom.City)
                .Include(c => c.Showroom.District)
                .Include(c => c.Showroom.WardsNavigation)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation)
                .FirstOrDefaultAsync(c => c.CarId == id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? CarId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var checkNotifi = _carRepo.GetAll().FirstOrDefault(c => c.CarId == CarId);
            checkNotifi.Notification = 2;
            checkNotifi.Note = Note;
            _carRepo.Update(checkNotifi);
            return RedirectToPage("./ListCar");
        }
    }
}
