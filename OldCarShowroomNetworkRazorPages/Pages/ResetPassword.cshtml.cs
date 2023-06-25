using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using System.Linq;
using BOs.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.ForgotPassword
{
    public class ResetPasswordModel : PageModel
    {
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;

        public ResetPasswordModel(UserRepository userRepo, INotyfService toastNotification)
        {
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }        
        [BindProperty]
        public string ConfirmNewPassword { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Key") != null) {
                Key = HttpContext.Session.GetString("Key");
                return Page();
            }
            Key = TempData["Email"] as string;
            return Page();
        }
        public IActionResult OnPost(string Key)
        {
            if (NewPassword == null)
            {
                Msg1 = "Nhập Mật khẩu mới";
                _toastNotification.Error("Reset mật khẩu thất bại");
                return Page();
            }
            if (ConfirmNewPassword == null)
            {
                Msg2 = "Nhập lại xác nhận mật khẩu";
                _toastNotification.Error("Reset mật khẩu thất bại");
                return Page();
            }
            if (!ConfirmNewPassword.Equals(NewPassword)) {
                Msg2 = "Nhập lại không đúng mật khẩu mới. Vui lòng nhập lại";
                _toastNotification.Error("Reset mật khẩu thất bại");
                return Page();
            }
            var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Key));
            if (checkEmail != null) { 
                checkEmail.Password = ConfirmNewPassword;
                _userRepo.Update(checkEmail);
                _toastNotification.Success("Reset mật khẩu thành công");
            }
            return RedirectToPage("./Login");
        }
    }
}
