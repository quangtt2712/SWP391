using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BOs.Models;
using REPOs;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OldCarShowroomNetworkRazorPages.Pages.Admin
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;
        public string Msg4 { get; set; }
        public string Msg5 { get; set; }
        public string Msg6 { get; set; }
        public EditModel(BOs.Models.OldCarShowroomNetworkContext context, UserRepository userRepo, INotyfService toastNotification)
        {
            _context = context;
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        [BindProperty]
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
           ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
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
            
            
            if (User.Password == null)
            {
                Msg5 = "không được để trống mật khẩu";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (User.FullName == null)
            {
                User.FullName = "";
            }
            if (User.Address == null)
            {
                User.Address = "";
            }
            if (User.Phone == null)
            {
                User.Phone = "";
            }
           

            User.RoleId = 2;
            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
