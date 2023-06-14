using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class EditModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarId { get; set; }
        public EditModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        [BindProperty]
        public BOs.Models.Car Car { get; set; }
        [BindProperty]
        public IFormFile UploadImg { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car = await _context.Cars
                .Include(c => c.CarModelYearNavigation)
                .Include(c => c.CarNameNavigation)
                .Include(c => c.ColorInsideNavigation)
                .Include(c => c.ColorNavigation)
                .Include(c => c.DriveNavigation)
                .Include(c => c.FuelNavigation)
                /*.Include(c => c.ImageCarNavigation)*/
                .Include(c => c.ManufactoryNavigation)
                .Include(c => c.UsernameNavigation)
                .Include(c => c.VehiclesNavigation).FirstOrDefaultAsync(m => m.CarId == id);

            if (Car == null)
            {
                return NotFound();
            }
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile UploadImg)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (UploadImg != null && UploadImg.Length > 0)
            {
                // Xóa hình ảnh hiện tại của showroom


                // Lưu trữ hình ảnh mới vào thư mục hoặc dịch vụ lưu trữ của bạn
                string imagePath = "wwwroot/images/car" + Guid.NewGuid().ToString() + "_" + UploadImg.FileName;
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await UploadImg.CopyToAsync(stream);
                }

                // Tạo đối tượng ImageShowroom và lưu thông tin ảnh vào cơ sở dữ liệu
                ImageCar newImage = new ImageCar
                {
                    Url = imagePath.Replace("wwwroot", "")
                };
                _context.ImageCars.Add(newImage);
                await _context.SaveChangesAsync();

                // Cập nhật ImageId của Showroom với ImageId mới
/*                Car.ImageCar = newImage.ImageId;
*/            }
            string userLogin = HttpContext.Session.GetString("Key");
            var user = _context.Users.FirstOrDefault(s => s.Email.Equals(userLogin));

            Car.Username = user.Username;
            _context.Attach(Car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(Car.CarId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
