using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CreateModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        public IActionResult OnGet()
        {
        ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name");
        ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "Name");
        ViewData["ImageId"] = new SelectList(_context.ImageShowrooms, "ImageId", "ImageId");
        ViewData["Wards"] = new SelectList(_context.Wards, "WardId", "Name");
            return Page();
        }

        [BindProperty]
        public BOs.Models.Showroom Showroom { get; set; }

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
                string imagePath = "wwwroot/images/showroom" + Guid.NewGuid().ToString() + "_" + uploadimg.FileName;
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await uploadimg.CopyToAsync(stream);
                }

                // Tạo đối tượng ImageShowroom và lưu thông tin ảnh vào cơ sở dữ liệu
                ImageShowroom image = new ImageShowroom
                {
                    Url = imagePath.Replace("wwwroot", "")
                };
                _context.ImageShowrooms.Add(image);
                await _context.SaveChangesAsync();

                // Gán ImageId của Showroom bằng ImageId mới được tạo ra
                Showroom.ImageId = image.ImageId;
            }

            _context.Showrooms.Add(Showroom);
            await _context.SaveChangesAsync();
           
            return RedirectToPage("./Index");
        }
    }
}
