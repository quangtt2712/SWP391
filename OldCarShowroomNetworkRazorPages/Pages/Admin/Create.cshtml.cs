using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using REPOs;
using OldCarShowroomNetworkRazorPages.Api;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
		public readonly UserRepository _userRepo;
		public string Msg4 { get; set; }
		public string Msg5 { get; set; }
        public string Msg6 { get; set; }
        private readonly INotyfService _toastNotification;
		public CreateModel(BOs.Models.OldCarShowroomNetworkContext context, UserRepository userRepo, INotyfService toastNotification)
        {
            _context = context;
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
        ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return Page();
        }

        [BindProperty]
        public BOs.Models.User User { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
			var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(User.Email));
            var checkUserName = _userRepo.GetAll().FirstOrDefault(p => p.Username.Equals(User.Username));
            if (checkEmail != null)
			{
				Msg4 = "Email đã tồn tại";
				_toastNotification.Error("Đăng kí thất bại");
				return Page();
			}
            if (checkUserName != null)
            {
                Msg6 = "Username đã tồn tại";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (User.Email == null)
            {
                Msg4 = "không được để trống email";
                _toastNotification.Error("Đăng kí thất bại");
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
            if (User.Username == null)
            {
                Msg6 = "không được để trống user";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }

            User.RoleId = 2;

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
