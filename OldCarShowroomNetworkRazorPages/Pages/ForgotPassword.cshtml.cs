using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OldCarShowroomNetworkRazorPages.Pages.ForgotPassword
{
    public class ForgotPasswordModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public ForgotPasswordModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

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
                return Page();
            }
            var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Email));
            if (checkEmail == null)
            {
                Msg1 = "Email không tồn tại";
                return Page();
            }
            return RedirectToPage("./ResetPassword");
        }
    }
}
