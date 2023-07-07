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
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Text.RegularExpressions;

namespace OldCarShowroomNetworkRazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
		public readonly UserRepository _userRepo;
		public string Msg4 { get; set; }
		public string MsgPhone { get; set; }
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

        [BindProperty]
        public string PasswordTemp { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

			
			if (!ModelState.IsValid)
            {
				
				return Page();
            }
			var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(User.Email));
            var checkPhone = _userRepo.GetAll().FirstOrDefault(p => p.Phone.Equals(User.Phone));
            var checkUserName = _userRepo.GetAll().FirstOrDefault(p => p.Username.Equals(User.Username));
            
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
            if (!string.IsNullOrEmpty(User.Username) && User.Username.Length > 126)
            {
                ModelState.AddModelError("User.Username", "Tên người dùng không được vượt quá 126 kí tự.");
                return Page();
            }

            if (checkUserName != null)
            {
                Msg6 = "Username đã tồn tại";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
          
            if (!string.IsNullOrEmpty(User.Password) && User.Password.Length > 16)
            {
                ModelState.AddModelError("User.Password", "Mật khẩu không được vượt quá 16 kí tự.");
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
			
			_context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
