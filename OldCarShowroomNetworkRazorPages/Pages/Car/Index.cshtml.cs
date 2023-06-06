using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{

    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public IndexModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        public IList<BOs.Models.Car> Car { get; set; }

        public async Task OnGetAsync()
        {
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
                .Include(c => c.VehiclesNavigation)
                .Join(_context.ImageCars,
                    car => car.ImageCar,
                    image => image.ImageId,
                    (car, image) => new
                    {
                        Car = car,
                        ImageUrl = image.Url
                    })
                .Select(x => new BOs.Models.Car
                {
                    CarId = x.Car.CarId,
                    Price = x.Car.Price,
                    CarModelYearNavigation = x.Car.CarModelYearNavigation,
                    CarNameNavigation = x.Car.CarNameNavigation,
                    ColorInsideNavigation = x.Car.ColorInsideNavigation,
                    ColorNavigation = x.Car.ColorNavigation,
                    DriveNavigation = x.Car.DriveNavigation,
                    FuelNavigation = x.Car.FuelNavigation,
                    ImageCarNavigation = new BOs.Models.ImageCar
                    {
                        ImageId = x.Car.ImageCarNavigation.ImageId,
                        Url = x.ImageUrl
                    },
                    ManufactoryNavigation = x.Car.ManufactoryNavigation,
                    UsernameNavigation = x.Car.UsernameNavigation,
                    VehiclesNavigation = x.Car.VehiclesNavigation
                })
                .ToListAsync();
        }
    }
}
