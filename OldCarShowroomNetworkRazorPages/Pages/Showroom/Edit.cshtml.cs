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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        private readonly INotyfService _toastNotification;

        public EditModel(OldCarShowroomNetworkContext context, INotyfService toastNotification)
        {
            _context = new OldCarShowroomNetworkContext();
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public BOs.Models.Showroom Showroom { get; set; }
        [BindProperty]
        public BOs.Models.ImageShowroom ImageShowroom { get; set; }

        public IList<BOs.Models.ImageShowroom> ImageShowrooms { get; set; }

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
                /*   .Include(s => s.Image)*/
                .Include(s => s.WardsNavigation).FirstOrDefaultAsync(m => m.ShowroomId == id);
            ImageShowroom = await _context.ImageShowrooms
               .FirstOrDefaultAsync(img => img.ShowroomId == id && img.ImageMain == true);

            ImageShowrooms = await _context.ImageShowrooms
                .Where(img => img.ShowroomId == id && img.ImageMain == false)
                .ToListAsync();

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
        public async Task<IActionResult> OnPostAsync(IFormFile uploadimg, IFormFile uploadimgmain)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Chỉnh sửa showroom thất bại");
                return Page();
            }

            _context.Attach(Showroom).State = EntityState.Modified;

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
                    Url = imagePath.Replace("wwwroot", ""),
                    ImageMain = false,
                    ShowroomId = Showroom.ShowroomId
                };
                _context.ImageShowrooms.Add(image);
               
            }
            if (uploadimgmain != null && uploadimgmain.Length > 0)
            {
                var oldMainImage = await _context.ImageShowrooms.FirstOrDefaultAsync(img => img.ShowroomId == Showroom.ShowroomId && img.ImageMain == true);
                if (oldMainImage != null)
                {
                    _context.ImageShowrooms.Remove(oldMainImage);
                }
                // Lưu trữ file ảnh vào thư mục hoặc dịch vụ lưu trữ của bạn
                // Ví dụ: sử dụng thư viện FileHelper để lưu trữ ảnh trong thư mục "wwwroot/images/showroom"
                string imagePath = "wwwroot/images/showroom" + Guid.NewGuid().ToString() + "_" + uploadimgmain.FileName;
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await uploadimgmain.CopyToAsync(stream);
                }

                // Tạo đối tượng ImageShowroom và lưu thông tin ảnh vào cơ sở dữ liệu
                ImageShowroom image = new ImageShowroom
                {
                    Url = imagePath.Replace("wwwroot", ""),
                    ImageMain = true,
                    ShowroomId = Showroom.ShowroomId
                };
                _context.ImageShowrooms.Add(image);
           
            }


 

            try
            {
                await _context.SaveChangesAsync();
                _toastNotification.Success("Chỉnh sửa showroom thành công");
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
