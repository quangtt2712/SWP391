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
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            ClaimsIdentity identity = null;

            if (Email == null)
            {
                Msg1 = "Nhập tài khoản";
                return Page();
            }

            if (Password == null)
            {
                Msg2 = "Nhập Mật khẩu";
                return Page();
            }

            var checkLoginByEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Email) && p.Password.Equals(Password));

            if (checkLoginByEmail == null)
            {
                Msg1 = "Tài khoản không tồn tại";
                return Page();
            }

            if (checkLoginByEmail.RoleId.Equals(0))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,checkLoginByEmail.Email),
                    new Claim(ClaimTypes.Role,"Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Key", Email);
                HttpContext.Session.SetString("Role", checkLoginByEmail.RoleId.ToString());
                return RedirectToPage("./User/Index");
            }

            if (checkLoginByEmail.RoleId.Equals(1))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,checkLoginByEmail.Email),
                    new Claim(ClaimTypes.Role,"User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Key", Email);
                HttpContext.Session.SetString("Role", checkLoginByEmail.RoleId.ToString());
                return RedirectToPage("./Index");
            }

            if (checkLoginByEmail.RoleId.Equals(2))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,checkLoginByEmail.Email),
                    new Claim(ClaimTypes.Role,"Staff")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Key", Email);
                HttpContext.Session.SetString("Role", checkLoginByEmail.RoleId.ToString());
                return RedirectToPage("/Showroom/Index");
            }
            return Page();
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
