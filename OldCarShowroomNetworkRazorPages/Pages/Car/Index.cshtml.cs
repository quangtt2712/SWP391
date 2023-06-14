﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{

    public class IndexModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public IndexModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        public IList<BOs.Models.Car> Car { get; set; }

        public IList<BOs.Models.ImageCar> ImageCar { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userLogin = HttpContext.Session.GetString("Key");
            if (string.IsNullOrEmpty(userLogin))
            {
                return Redirect("/Login");
            }
            var user = _context.Users.FirstOrDefault(s => s.Email.Equals(userLogin));

            Car = await _context.Cars
                 .Include(c => c.CarModelYearNavigation)
                 .Include(c => c.CarNameNavigation)
                 .Include(c => c.ColorInsideNavigation)
                 .Include(c => c.ColorNavigation)
                 .Include(c => c.DriveNavigation)
                 .Include(c => c.FuelNavigation)
                 .Include(c => c.ManufactoryNavigation)
                 .Include(c => c.Showroom)
                 .Include(c => c.UsernameNavigation)
                 .Include(c => c.VehiclesNavigation).ToListAsync();
            ImageCar = await _context.ImageCars.ToListAsync();
            return Page();
        }
    }
}
