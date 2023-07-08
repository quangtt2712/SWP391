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
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPage.Pages
{
    public class LoginModel : PageModel
    {
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(UserRepository userRepo, INotyfService toastNotification, ILogger<LoginModel> logger)
        {
            _userRepo = userRepo;
            _toastNotification = toastNotification;
            _logger = logger;
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
                _toastNotification.Error("Đăng nhập thất bại");
                return Page();
            }

            if (Password == null)
            {
                Msg2 = "Nhập Mật khẩu";
                _toastNotification.Error("Đăng nhập thất bại");
                return Page();
            }

            var checkLoginByEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(Email) && p.Password.Equals(Password));

            if (checkLoginByEmail == null)
            {
                Msg1 = "Tài khoản không tồn tại hoặc sai mật khẩu";
                _toastNotification.Error("Đăng nhập thất bại");
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
                _toastNotification.Success("Đăng nhập thành công");
                return RedirectToPage("./Admin/Index");
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
                _toastNotification.Success("Đăng nhập thành công");
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
                _toastNotification.Success("Đăng nhập thành công");
                return RedirectToPage("/Showroom/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostLogout()
        {
            HttpContext.Session.Remove("Key");
            HttpContext.Session.Remove("Role");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("./Index");
        }
        
    }
}
