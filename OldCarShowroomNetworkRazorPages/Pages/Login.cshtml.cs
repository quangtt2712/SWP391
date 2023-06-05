using BOs.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Google;

namespace OldCarShowroomNetworkRazorPage.Pages
{
    public class LoginModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public LoginModel(UserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }

        [BindProperty]
        public string Key { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            if (Key == null)
            {
                Msg1 = "Nhập tài khoản";
                return Page();
            }

            if (Password == null)
            {
                Msg2 = "Nhập Mật khẩu";
                return Page();
            }
            var checkLoginByEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Key) && p.Password.Equals(Password));
            //var checkLoginByPhone = _userRepo.GetAll().FirstOrDefault(p => p.Phone.Equals(Key) && p.Password.Equals(Password));
            //var checkLoginByUserName = _userRepo.GetAll().FirstOrDefault(p => p.Username.Equals(Key) && p.Password.Equals(Password));

            if (checkLoginByEmail == null)
            {
                Msg1 = "Tài khoản không tồn tại";
                return Page();
            }
            HttpContext.Session.SetString("Key", Key);
            HttpContext.Session.SetString("Role", checkLoginByEmail.RoleId.ToString());
            return RedirectToPage("./Index");
        }

        //public async Task Login()
        //{
        //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action("GoogleResponse")
        //    });
        //}


        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    if (result != null)
        //    {
        //        HttpContext.Session.SetString("Key", Key);
        //        var scheme = CookieAuthenticationDefaults.AuthenticationScheme;

        //        var User = new ClaimsPrincipal(
        //            new ClaimsIdentity(
        //                new[] { new Claim(ClaimTypes.Name, Key) },
        //                scheme
        //                ));
        //        return RedirectToPage("./Home");
        //    }
        //    return Page();
        //}
    }
}
