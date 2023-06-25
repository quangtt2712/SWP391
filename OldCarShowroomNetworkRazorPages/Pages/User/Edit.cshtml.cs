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

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User,Staff")]
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
        public string Msg5 { get; set; }
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
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg1 = "Cần nhập thông tin để chỉnh sửa";
                return Page();
            }
            if (user.FullName != null) {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg2 = "Cần nhập Tên để chỉnh sửa";
                return Page();
            }
            if (user.Address != null)
            {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg2 = "Cần nhập địa chỉ để chỉnh sửa";
                return Page();
            }
            if (user.Email != null)
            {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg2 = "Cần nhập email để chỉnh sửa";
                return Page();
            }
            if (user.Phone != null)
            {
                _toastNotification.Error("Chỉnh sửa thông tin thất bại");
                Msg2 = "Cần nhập SĐT để chỉnh sửa";
                return Page();
            }
            _userRepo.Update(user);
            _toastNotification.Success("Chỉnh sửa thông tin thành công");
            return RedirectToPage("./Details");
        }
    }
}
