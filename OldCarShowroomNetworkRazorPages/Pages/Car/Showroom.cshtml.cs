using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    public class ShowroomModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public ShowroomModel()
        {
            _context = new OldCarShowroomNetworkContext();
        }

        public IList<BOs.Models.Showroom> Showroom { get;set; }

        public async Task OnGetAsync()
        {
            ViewData["city"] = new SelectList(_context.Cities, "CityId", "Name");
            ViewData["district"] = new SelectList(_context.Districts, "DistrictId", "Name");
            ViewData["ward"] = new SelectList(_context.Wards, "WardId", "Name");
            Showroom = await _context.Showrooms
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.Image)
                .Include(s => s.WardsNavigation).ToListAsync();
        }
    }
}
