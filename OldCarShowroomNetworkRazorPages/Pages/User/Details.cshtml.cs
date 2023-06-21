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
using Microsoft.AspNetCore.Http;
using REPOs;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User,Staff")]
    public class DetailsModel : PageModel
    {
        public readonly UserRepository _userRepo;

        public DetailsModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public BOs.Models.User user { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Email = HttpContext.Session.GetString("Key");
            user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));
            return Page();
        }
    }
}
