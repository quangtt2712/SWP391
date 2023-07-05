using AspNetCoreHero.ToastNotification.Abstractions;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OldCarShowroomNetworkRazorPages.Pagination;
using REPOs;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class ListCarcshtmlModel : PageModel
    {
        public readonly CarRepository _carRepo;
        private readonly INotyfService _toastNotification;

        public ListCarcshtmlModel(CarRepository carRepo, INotyfService toastNotification)
        {
            _carRepo = carRepo;
            _toastNotification = toastNotification;
        }

        public PaginatedList<BOs.Models.Car> car { get; set; }
        public string Msg1 { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var pageSize = 8;
            var list = from c in _carRepo.GetAll()
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
                .Where(c => c.Notification.Equals(0))
                .OrderByDescending(c => c.CreatedAt)
                select c;
            car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);

            if (car.Count() == 0 || car == null)
            {
                Msg1 = "Hiện tại không có xe chờ kí gửi";
                return Page();
            }
            return Page();
        }
    }
}
