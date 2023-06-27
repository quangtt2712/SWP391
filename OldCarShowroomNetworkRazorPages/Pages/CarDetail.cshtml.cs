﻿using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class CarDetailModel : PageModel
    {
        public readonly CarRepository _carRepo;
        public readonly BookingRepository _bookRepo;
        public readonly UserRepository _userkRepo;

        public CarDetailModel(CarRepository carRepo, BookingRepository bookRepo, UserRepository userkRepo)
        {
            _carRepo = carRepo;
            _bookRepo = bookRepo;
            _userkRepo = userkRepo;
        }

        public BOs.Models.Car car { get; set; }
        public BOs.Models.User user { get; set; }
        public string email { get; set; }
        public string Msg { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("Key") == null)
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

            }
            if (HttpContext.Session.GetString("Key") != null && HttpContext.Session.GetString("Role") != null)
            {
                email = HttpContext.Session.GetString("Key");
                user = await _userkRepo.GetAll().FirstOrDefaultAsync(u => u.Email == email);
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
                    var checkBooking = await _bookRepo.GetAll().FirstOrDefaultAsync(b => b.CarId == id && b.Username == user.Username);
                    if (checkBooking != null)
                    {
                        Msg = "Bạn đã đặt lịch xem xe này rồi mời bạn xem xe khác";
                        return Page();
                    }
                }
            }
            return Page();
        }
    }
}
