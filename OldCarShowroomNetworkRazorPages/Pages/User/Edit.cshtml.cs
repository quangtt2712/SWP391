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
using REPOs;
using Microsoft.AspNetCore.Http;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User,Staff")]
    public class EditModel : PageModel
    {
        public readonly UserRepository _userRepo;

        public EditModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public BOs.Models.User user { get; set; }
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Email = HttpContext.Session.GetString("Key");
            user = await _userRepo.GetAll().FirstOrDefaultAsync(u => u.Email.Equals(Email));

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _userRepo.Update(user);

            return RedirectToPage("./Details");
        }
    }
}
