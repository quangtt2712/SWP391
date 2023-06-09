using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CreateModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        
        public IActionResult OnGet()
        {
        ViewData["CarModelYear"] = new SelectList(_context.CarModelYears, "CarModelYearId", "CarModelYear1");
        ViewData["CarName"] = new SelectList(_context.CarNames, "CarNameId", "CarName1");
        ViewData["ColorInside"] = new SelectList(_context.Colors, "ColorId", "ColorName");
        ViewData["Color"] = new SelectList(_context.Colors, "ColorId", "ColorName");
        ViewData["Drive"] = new SelectList(_context.Drives, "DriveId", "DriveName");
        ViewData["Fuel"] = new SelectList(_context.Fuels, "FuelId", "FuelName");
        ViewData["ImageCar"] = new SelectList(_context.ImageCars, "ImageId", "ImageId");
        ViewData["Manufactory"] = new SelectList(_context.Manufactorys, "ManufactoryId", "ManufactoryName");
        ViewData["Username"] = new SelectList(_context.Users, "Username", "FullName");
        ViewData["Vehicles"] = new SelectList(_context.Vehicles, "VehiclesId", "VehiclesName");
            return Page();
        }

        [BindProperty]
        public BOs.Models.Car Car { get; set; }

        [BindProperty]
        public BOs.Models.User User { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile uploadimg)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Kiểm tra xem có file ảnh được tải lên hay không
            if (uploadimg != null && uploadimg.Length > 0)
            {
                // Lưu trữ file ảnh vào thư mục hoặc dịch vụ lưu trữ của bạn
                // Ví dụ: sử dụng thư viện FileHelper để lưu trữ ảnh trong thư mục "wwwroot/images/showroom"
                string imagePath = "wwwroot/images/car" + Guid.NewGuid().ToString() + "_" + uploadimg.FileName;
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await uploadimg.CopyToAsync(stream);
                }

                // Tạo đối tượng ImageShowroom và lưu thông tin ảnh vào cơ sở dữ liệu
                ImageCar image = new ImageCar
                {
                    Url = imagePath.Replace("wwwroot", "")
                };
                _context.ImageCars.Add(image);
                await _context.SaveChangesAsync();

                // Gán ImageId của Showroom bằng ImageId mới được tạo ra
                Car.ImageCar = image.ImageId;
            }
            string userLogin = HttpContext.Session.GetString("Key");
            var user = _context.Users.FirstOrDefault(s => s.Email.Equals(userLogin));

            Car.Username = user.Username;
            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
