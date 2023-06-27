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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        private readonly INotyfService _toastNotification;

        public DeleteModel(OldCarShowroomNetworkContext context, INotyfService toastNotification)
        {
            _context = new OldCarShowroomNetworkContext();
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public BOs.Models.Showroom Showroom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showroom = await _context.Showrooms
                .Include(s => s.City)
                .Include(s => s.District)
              /*  .Include(s => s.Image)*/
                .Include(s => s.WardsNavigation).FirstOrDefaultAsync(m => m.ShowroomId == id);

            if (Showroom == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Showroom = await _context.Showrooms.Include(s => s.Cars).FirstOrDefaultAsync(s => s.ShowroomId == id);

            if (Showroom != null)
            {
                var associatedImages = await _context.ImageShowrooms
                .Where(img => img.ShowroomId == Showroom.ShowroomId)
                .ToListAsync();

                _context.ImageShowrooms.RemoveRange(associatedImages);
                await _context.SaveChangesAsync();
                if (Showroom.Cars.Any())
                {
                    Showroom.Cars.ToList().ForEach(c =>
                    {
                        c.Showroom = null;
                        _context.Cars.Update(c);
                        _context.SaveChanges();
                    });
                }
                Showroom.Cars= null;
                _context.Showrooms.Remove(Showroom);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Xóa showroom thành công");
            }
            return RedirectToPage("./Index");
        }
    }
}
