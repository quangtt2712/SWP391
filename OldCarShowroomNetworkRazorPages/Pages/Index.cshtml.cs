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

namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly CarRepository _carRepo;
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        [BindProperty]
        public string Key { get; set; }
        public string Role { get; set; }
        public string Msg { get; set; }
        public string Msg1 { get; set; }

        public IList<BOs.Models.Car> car { get; set; }
        [BindProperty]
        public string searchKey { get; set; }

        public IndexModel(CarRepository carRepo, OldCarShowroomNetworkContext context)
        {
            _carRepo = carRepo;
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Key") == null || HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {
                if (_carRepo.GetAll() != null)
                {
                    car =  _carRepo.GetAll()
                        .Include(c => c.CarModelYearNavigation)
                        .Include(c => c.CarNameNavigation)
                        .Include(c => c.ColorInsideNavigation)
                        .Include(c => c.ColorNavigation)
                        .Include(c => c.DriveNavigation)
                        .Include(c => c.FuelNavigation)
                        .Include(c => c.ManufactoryNavigation)
                        .Include(c => c.Showroom)
                        .Include(c => c.UsernameNavigation)
                        .Include(c => c.VehiclesNavigation).ToList();
                }
                return Page();
            }
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost()
        {
            if (searchKey == null)
            {
                Msg1 = "Vui lòng nhập tên xe để tìm kiếm";
                return Page();
            }
            var checkCar = _carRepo.GetAll()
                        .Include(c => c.CarModelYearNavigation)
                        .Include(c => c.CarNameNavigation)
                        .Include(c => c.ColorInsideNavigation)
                        .Include(c => c.ColorNavigation)
                        .Include(c => c.DriveNavigation)
                        .Include(c => c.FuelNavigation)
                        .Include(c => c.ManufactoryNavigation)
                        .Include(c => c.Showroom)
                        .Include(c => c.UsernameNavigation)
                        .Include(c => c.VehiclesNavigation).Where(p => p.ManufactoryNavigation.ManufactoryName.ToLower().Contains(searchKey.ToLower()));
            if(checkCar == null)    
            {
                Msg1 = "Không tìm thấy xe";
                return Page();
            }
            car =  checkCar
                 .Include(c => c.CarModelYearNavigation)
                 .Include(c => c.CarNameNavigation)
                 .Include(c => c.ColorInsideNavigation)
                 .Include(c => c.ColorNavigation)
                 .Include(c => c.DriveNavigation)
                 .Include(c => c.FuelNavigation)
                 .Include(c => c.ManufactoryNavigation)
                 .Include(c => c.Showroom)
                 .Include(c => c.UsernameNavigation)
                 .Include(c => c.VehiclesNavigation).ToList();
            return Page();
        }


    }
}
