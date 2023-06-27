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

namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly CarRepository _carRepo;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(CarRepository carRepo, ILogger<IndexModel> logger)
        {
            _carRepo = carRepo;
            _logger = logger;
        }

        [BindProperty]
        public string Key { get; set; }
        public string Role { get; set; }
        public string Msg { get; set; }
        public string Msg1 ;

        public IList<BOs.Models.Car> car { get; set; }
        [BindProperty]
        public string searchKey { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Key") == null || HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
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
                    .Include(c => c.Showroom.City)
                    .Include(c => c.VehiclesNavigation)
                    .Include(c => c.Showroom.City)
					.Include(c => c.ImageCars)
					.Include(c => c.Showroom.District)
                    .Include(c => c.Showroom.WardsNavigation)
                    .Where(c =>c.Notification.Equals(1)).ToListAsync();
                
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                Msg1 = "Vui lòng nhập tên xe để tìm kiếm";
                return Page();
            }
            var checkCar = _carRepo.GetAll().Where(p => p.Notification.Equals(1) && p.ManufactoryNavigation.ManufactoryName.ToLower().Contains(searchKey.ToLower().Trim()) 
            || p.Notification.Equals(1) && p.CarNameNavigation.CarName1.ToLower().Contains(searchKey.ToLower()));
            if (checkCar.Count() == 0)
            {
                Msg1 = "Không tìm thấy xe";
                return Page();
            }
            car = await checkCar
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
                .Include(c => c.ImageCars)
                .Include(c => c.Showroom.WardsNavigation)
                .Where(c => c.Notification.Equals(1)).ToListAsync();
            return Page();
        }


    }
}
