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
using System.IO;
using Microsoft.AspNetCore.Http;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public EditModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        [BindProperty]
        public BOs.Models.Showroom Showroom { get; set; }
        [BindProperty]
        public IFormFile UploadImg { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showroom = await _context.Showrooms
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Image)
                .Include(s => s.WardsNavigation).FirstOrDefaultAsync(m => m.ShowroomId == id);

            if (Showroom == null)
            {
                return NotFound();
            }
           ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "Name");
           ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "Name");
           ViewData["ImageId"] = new SelectList(_context.ImageShowrooms, "ImageId", "ImageId");
           ViewData["Wards"] = new SelectList(_context.Wards, "WardId", "Name");
          

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
                string imagePath = "wwwroot/images/showroom" + Guid.NewGuid().ToString() + "_" + UploadImg.FileName;
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await UploadImg.CopyToAsync(stream);
                }

                // Tạo đối tượng ImageShowroom và lưu thông tin ảnh vào cơ sở dữ liệu
                ImageShowroom newImage = new ImageShowroom
                {
                    Url = imagePath.Replace("wwwroot", "")
                };
                _context.ImageShowrooms.Add(newImage);
                await _context.SaveChangesAsync();

                // Cập nhật ImageId của Showroom với ImageId mới
                Showroom.ImageId = newImage.ImageId;
            }

            _context.Attach(Showroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowroomExists(Showroom.ShowroomId))
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

        private bool ShowroomExists(int id)
        {
            return _context.Showrooms.Any(e => e.ShowroomId == id);
        }
    }
}
