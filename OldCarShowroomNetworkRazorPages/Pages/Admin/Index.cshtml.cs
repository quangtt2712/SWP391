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
	public class IndexModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public IndexModel(BOs.Models.OldCarShowroomNetworkContext context)
        {
            _context = context;
        }

        public IList<BOs.Models.User> User { get;set; }

        public async Task OnGetAsync()
        {
            User = await _context.Users
                .Include(u => u.Role).Where(u => u.RoleId == 2).ToListAsync();
        }
    }
}
