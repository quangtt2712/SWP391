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
using System.Text.RegularExpressions;

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
		public string MsgPhone { get; set; }

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
			var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(User.Email) && p.Username != User.Username);
			var checkPhone = _userRepo.GetAll().FirstOrDefault(p => p.Phone.Equals(User.Phone) && p.Username.Equals(User.Username) == false);
			if (checkEmail != null)
			{
				Msg4 = "Email đã tồn tại";
				_toastNotification.Error("Đăng kí thất bại");
				return Page();
			}
			if (checkPhone != null)
			{
				MsgPhone = "Phone đã tồn tại";
				_toastNotification.Error("Đăng kí thất bại");
				return Page();
			}
			if (User.Password == null)
            {
                Msg5 = "không được để trống mật khẩu";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
           
			if (!string.IsNullOrEmpty(User.Email) && User.Email.Length > 99)
			{
				ModelState.AddModelError("User.Email", "Email không được vượt quá 99 kí tự.");
				return Page();
			}

			if (!string.IsNullOrEmpty(User.Email) && !Regex.IsMatch(User.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
			{
				ModelState.AddModelError("User.Email", "Email không hợp lệ.");
				return Page();
			}

			if (!string.IsNullOrEmpty(User.Phone) && User.Phone.Length > 10)
			{
				ModelState.AddModelError("User.Phone", "Số điện thoại không được vượt quá 10 kí tự.");
				return Page();
			}

			if (!string.IsNullOrEmpty(User.Phone) && !Regex.IsMatch(User.Phone, @"(84|0[3|5|7|8|9])+([0-9]{8})\b"))
			{
				ModelState.AddModelError("User.Phone", "Số điện thoại không hợp lệ.");
				return Page();
			}

			if (!string.IsNullOrEmpty(User.Password) && User.Password.Length > 16)
			{
				ModelState.AddModelError("User.Password", "Mật khẩu không được vượt quá 16 kí tự.");
				return Page();
			}

			if (!string.IsNullOrEmpty(User.FullName) && User.FullName.Length > 50)
			{
				ModelState.AddModelError("User.FullName", "Tên không được vượt quá 50 kí tự.");
				return Page();
			}
			if (!string.IsNullOrEmpty(User.Address) && User.Address.Length > 255)
			{
				ModelState.AddModelError("User.Address", "Địa chỉ không được vượt quá 255 kí tự.");
				return Page();
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
