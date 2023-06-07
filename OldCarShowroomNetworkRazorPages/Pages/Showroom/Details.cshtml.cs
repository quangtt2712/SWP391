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
    public class DetailsModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public DetailsModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

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
    }
}
