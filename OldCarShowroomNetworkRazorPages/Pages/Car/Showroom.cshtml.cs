using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OldCarShowroomNetworkRazorPages.Pages.Car
{
    [Authorize(Roles = "User")]
    public class ShowroomModel : PageModel
	{
		private readonly BOs.Models.OldCarShowroomNetworkContext _context;
		[BindProperty(SupportsGet = true)]
		public string CityId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string DistrictId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string WardId { get; set; }

		public ShowroomModel()
		{
			_context = new OldCarShowroomNetworkContext();
		}

		public IList<BOs.Models.Showroom> Showroom { get; set; }
		public IList<BOs.Models.ImageShowroom> ImageShowroom { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{
            ViewData["city"] = new SelectList(_context.Cities, "CityId", "Name");
			ViewData["district"] = new SelectList(_context.Districts, "DistrictId", "Name");
			ViewData["ward"] = new SelectList(_context.Wards, "WardId", "Name");
			Showroom = await _context.Showrooms
				.Include(s => s.City)
				.Include(s => s.District)
				.Include(s => s.ImageShowrooms)
				.Include(s => s.WardsNavigation).ToListAsync();
			ImageShowroom = await _context.ImageShowrooms.ToListAsync();
            return Page();
		}
	}
}
