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

namespace OldCarShowroomNetworkRazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public DetailsModel(BOs.Models.OldCarShowroomNetworkContext context)
        {
            _context = context;
        }

        public BOs.Models.User User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.Users
                .Include(u => u.Role).FirstOrDefaultAsync(m => m.Username == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
