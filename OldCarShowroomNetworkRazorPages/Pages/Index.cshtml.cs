using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using REPOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOs.Models;
using Microsoft.EntityFrameworkCore;
using OldCarShowroomNetworkRazorPage.Pages;
using OldCarShowroomNetworkRazorPages.Pagination;
namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly CarRepository _carRepo;
        public readonly UserRepository _userRepo;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(CarRepository carRepo, UserRepository userRepo, ILogger<IndexModel> logger)
        {
            _carRepo = carRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        [BindProperty]
        public string Key { get; set; }
        public BOs.Models.User user { get; set; }
        public string Msg { get; set; }
        public string email { get; set; }
        public string Msg1 ;

        public PaginatedList<BOs.Models.Car> car { get; set; }
        public IList<BOs.Models.Car> checkList { get; set; }
        [BindProperty]
        public string searchKey { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var pageSize = 8;
            if (HttpContext.Session.GetString("Key") == null)
            {
                var list = from p in _carRepo.GetAll()
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
                    .Where(c => c.Notification.Equals(1))
                    .OrderByDescending(c => c.AcceptedAt)
                    select p;

                car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);
                return Page();

            }
            if (HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {
                email = HttpContext.Session.GetString("Key");
                user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email == email);
                var list = from p in _carRepo.GetAll()
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
                    .Include(c => c.Bookings.Where(b => b.Notification == 1))
                    .Where(c => c.Notification.Equals(1) && c.Username != user.Username && c.Bookings.Count() == 0)
                    .OrderByDescending(c => c.AcceptedAt)
                    select p;

                car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string searchKey, int? pageIndex)
        {
            var pageSize = 8;
            if (string.IsNullOrWhiteSpace(searchKey))
            {   
                Msg1 = "Vui lòng nhập tên xe hoặc hãng xe để tìm kiếm";
                return Page();
            }

            var checkCar = _carRepo.GetAll()
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.CarNameNavigation)
                .Where(p => p.Notification.Equals(1) && p.ManufactoryNavigation.ManufactoryName.ToLower().Contains(searchKey.ToLower().Trim()) 
                    || p.Notification.Equals(1) && p.CarNameNavigation.CarName1.ToLower().Contains(searchKey.ToLower()));

            car = await PaginatedList<BOs.Models.Car>.CreateAsync(checkCar, pageIndex ?? 1, pageSize);

            if (car.Count() == 0 || car == null)
            {
                Msg1 = "Không tìm thấy xe";
                return Page();
            }
			if (HttpContext.Session.GetString("Key") == null)
			{
                var list = from p in checkCar.Include(c => c.ImageCars)
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
                    .Where(c => c.Notification.Equals(1))
                    .OrderByDescending(c => c.AcceptedAt)
                    select p;

                car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);
                return Page();
			}
			if (HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {
                email = HttpContext.Session.GetString("Key");
                user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email == email);

                var list = from p in checkCar.Include(c => c.ImageCars)
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
                   .Where(c => c.Notification.Equals(1) && c.Username != user.Username)
                   .OrderByDescending(c => c.AcceptedAt)
                   select p;

                car = await PaginatedList<BOs.Models.Car>.CreateAsync(list, pageIndex ?? 1, pageSize);
                return Page();
			}
			return Page();
        }


    }
}
