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
using Microsoft.AspNetCore.Hosting;
using OldCarShowroomNetworkRazorPages.Pagination;

namespace OldCarShowroomNetworkRazorPages.Pages.Showroom
{
    public class IndexModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IndexModel(IWebHostEnvironment webHostEnvironment)
        {
            _context = new OldCarShowroomNetworkContext();
            _webHostEnvironment = webHostEnvironment;
        }
       

        public PaginatedList<BOs.Models.Showroom> Showroom { get;set; }
        public async Task OnGetAsync(int? pageIndex)
        {
            var pageSize = 8;
            var list = from s in _context.Showrooms
                .Include(s => s.ImageShowrooms)
                .Include(s => s.City)
                .Include(s => s.District)
                .Include(s => s.WardsNavigation)
                select s;
            Showroom = await PaginatedList<BOs.Models.Showroom>.CreateAsync(list, pageIndex ?? 1, pageSize);
        }
     
    }
}
