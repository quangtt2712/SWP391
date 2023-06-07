using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using System.Linq;
using BOs.Models;
using System.Collections.Generic;

namespace OldCarShowroomNetworkRazorPages.Pages.ForgotPassword
{
    public class ResetPasswordModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }        
        [BindProperty]
        public string ConfirmNewPassword { get; set; }
        public string Key { get; set; }

        public ResetPasswordModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {

            if (NewPassword == null)
            {
                Msg1 = "Nhập Mật khẩu mới";
                return Page();
            }
            if (ConfirmNewPassword == null)
            {
                Msg2 = "Nhập lại mật khẩu";
                return Page();
            }
            if (!ConfirmNewPassword.Equals(NewPassword)) {
                Msg2 = "Nhập lại không đúng mật khẩu mới. Vui lòng nhập lại";
                return Page();
            }
            var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Key));
            checkEmail.Password = ConfirmNewPassword;
            _userRepo.Update(checkEmail);
            return RedirectToPage("./Login");
        }
    }
}
