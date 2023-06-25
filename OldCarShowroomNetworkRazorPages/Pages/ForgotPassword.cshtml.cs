using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPages.Pages.ForgotPassword
{
    public class ForgotPasswordModel : PageModel
    {
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;

        public ForgotPasswordModel(UserRepository userRepo, INotyfService toastNotification)
        {
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {

            if (Email == null)
            {
                Msg1 = "Nhập Email";
                _toastNotification.Error("Xác nhận Email thất bại");
                return Page();
            }
            var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Email));
            if (checkEmail == null)
            {
                Msg1 = "Email không tồn tại";
                _toastNotification.Error("Xác nhận Email thất bại");
                return Page();
            }
            TempData["Email"] = Email;
            return RedirectToPage("./ResetPassword");
        }
    }
}
