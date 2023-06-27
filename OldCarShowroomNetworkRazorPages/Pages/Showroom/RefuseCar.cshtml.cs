using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _toastNotification;

        public RefuseCarModel(CarRepository carRepo, INotyfService toastNotification)
        {
            _carRepo = carRepo;
            _toastNotification = toastNotification;
        }
        public string Msg { get; set; }
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
                .FirstOrDefaultAsync(c => c.CarId == CarId);

            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Từ chối kí gửi xe thất bại");
                return Page();
            }
            if (Note == null)
            {
                Msg = "Cần nhập lý do để từ chối kí gửi xe";
                _toastNotification.Error("Từ chối kí gửi xe thất bại");
                return Page();
            }
            var checkNotifi = _carRepo.GetAll().FirstOrDefault(c => c.CarId == CarId);
            if (checkNotifi != null) { 
                checkNotifi.Notification = 2;
                checkNotifi.Note = Note;
                _carRepo.Update(checkNotifi);
                _toastNotification.Success("Từ chối kí gửi xe thành công");
            }
            return RedirectToPage("./ListCar");
        }
    }
}
