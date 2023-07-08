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
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Text.RegularExpressions;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User,Staff, Admin")]
    public class EditModel : PageModel
    {
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;

        public EditModel(UserRepository userRepo, INotyfService toastNotification)
        {
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public BOs.Models.User user { get; set; }
        public string Email { get; set; }
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string Msg3 { get; set; }
        public string Msg4 { get; set; }
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

            if (user.FullName == null) {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg2 = "Cần nhập Tên để chỉnh sửa";
                return Page();
            }
            if (user.Address == null)
            {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg3 = "Cần nhập địa chỉ để chỉnh sửa";
                return Page();
            }
            if (!string.IsNullOrEmpty(user.Phone) && user.Phone.Length > 10)
            {
                ModelState.AddModelError("user.Phone", "Số điện thoại không được vượt quá 10 kí tự.");
                return Page();
            }

            if (!string.IsNullOrEmpty(user.Phone) && !Regex.IsMatch(user.Phone, @"(84|0[3|5|7|8|9])+([0-9]{8})\b"))
            {
                ModelState.AddModelError("user.Phone", "Số điện thoại không hợp lệ.");
                return Page();
            }
            if (user.Phone == null)
            {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg4 = "Cần nhập SĐT để chỉnh sửa";
                return Page();
            }
            if (user.Password == null) 
            {
                user.Password = "";
            }
            _userRepo.Update(user);
            _toastNotification.Success("Chỉnh sửa thông tin thành công");
            return RedirectToPage("./Details");
        }
    }
}
