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

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public DeleteModel()
        {
            _context = new OldCarShowroomNetworkContext();
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
                .Include(s => s.Image)
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

            Showroom = await _context.Showrooms.FindAsync(id);

            if (Showroom != null)
            {
                _context.Showrooms.Remove(Showroom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
